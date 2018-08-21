/* eslint-disable */
const fs = require('fs');
const del = require('del');
const cpy = require('cpy');
const path = require('path');
const webpack = require('webpack');
const cp = require('child_process');

const tasks = new Map();

function run(task) {
  const start = new Date();
  console.log(`Starting '${task}'...`);
  return Promise.resolve().then(() => tasks.get(task)()).then(() => {
    console.log(`Finished '${task}' after ${new Date().getTime() - start.getTime()}ms`);
  }, err => console.error(err.stack));
}

//
// Clean up the output directory
// -----------------------------------------------------------------------------
tasks.set('clean', () => Promise.resolve()
  .then(() => del(['coverage/*', 'wwwroot/dist', 'server/bin/*'], { dot: true }))
);

//
// Copy ASP.NET application config file for production and development environments
// -----------------------------------------------------------------------------
tasks.set('appsettings', () => new Promise(resolve => {
  const environments = ['Production', 'Development'];
  let count = environments.length;
  const source = require('./server/appsettings.json');
  delete source.Logging;
  environments.forEach(env => {
    const filename = path.resolve(__dirname, `./server/appsettings.${env}.json`);
    try {
      fs.writeFileSync(filename, JSON.stringify(source, null, '  '), { flag: 'wx' });
    } catch (err) {}
    if (--count === 0) resolve();
  });
}));

//
// Build server and client application
// -----------------------------------------------------------------------------
tasks.set('build', () => {
  global.DEBUG = process.argv.includes('--debug') || false;
  return Promise.resolve()
    .then(() => run('appsettings'))
    .then(() => new Promise((resolve, reject) => {
      const options = { stdio: ['ignore', 'inherit', 'inherit'] };
      const config = global.DEBUG ? 'Debug' : 'Release';
      //const args = ['publish', 'server', '-o', '../build', '-c', config];
      const args = ['build', 'server', '-c', config];
      cp.spawn('dotnet', args, options).on('close', code => {
        if (code === 0) {
          resolve();
        } else {
          reject(new Error(`dotnet ${args.join(' ')} => ${code} (error)`));
        }
      });
    }));
});

//
// Build website and launch it in a browser for testing in watch mode
// -----------------------------------------------------------------------------
tasks.set('start', () => {
  global.HMR = !process.argv.includes('--no-hmr'); // Hot Module Replacement (HMR)
  return Promise.resolve()
    .then(() => run('clean'))
    .then(() => run('appsettings'))
    .then(() => run('build'))
    .then(() => new Promise(resolve => {
      let count = 0;
      const webpackConfig = require('./webpack.config');
      const compiler = webpack(webpackConfig);

      // Node.js middleware that compiles application in watch mode with HMR support
      // http://webpack.github.io/docs/webpack-dev-middleware.html
      const webpackDevMiddleware = require('webpack-dev-middleware')(compiler, {
        publicPath: webpackConfig.output.publicPath,
        stats: webpackConfig.stats
      });
      
      compiler.hooks.done.tap('done', () => {
        process.stdout.write(webpackConfig.output.publicPath + "\r\n");

        // Launch ASP.NET Core server after the initial bundling is complete
        if (++count === 1) {
          const options = {
            cwd: path.resolve(__dirname, './server/'),
            stdio: ['ignore', 'pipe', 'inherit'],
            env: Object.assign({}, process.env, {
              ASPNETCORE_ENVIRONMENT: 'Development',
              NODE_PATH: '../node_modules/'
            }),
          };
          cp.spawn('dotnet', ['watch', 'run'], options).stdout.on('data', data => {
            process.stdout.write(data);
          });
        }
      });
    }));
});

// Execute the specified task or default one. E.g.: node run build
run(/^\w/.test(process.argv[2] || '') ? process.argv[2] : 'start' /* default */);
