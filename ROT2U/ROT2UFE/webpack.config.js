var path = require('path');
var webpack = require('webpack');

var ForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin');

module.exports = {
	stats: "errors-only",
	watchOptions: {	
		ignored: /node_modules/
	},
	//devtool: "eval-source-map",
	entry: {
		client: ['babel-polyfill', "./client/boot-client.tsx"],
	},
	output: {
		publicPath: path.resolve(__dirname,"./wwwroot/dist"),
		path: path.resolve(__dirname,"./wwwroot/dist"),
		filename: "[name].js"
	},
	module: {
		rules: [
			{
				test: /\.tsx?$/,
				include: [path.resolve(__dirname, "client")],
            	use: [
					  { loader: 'babel-loader?presets[]=es2015'},
					  { loader: 'ts-loader',
					  options: {
						transpileOnly: true // IMPORTANT! use transpileOnly mode to speed-up compilation
					  }
					}
				],
			},
			{
				test: /\.css$/,
				use: [ 'style-loader', 'css-loader' ]
			},
			{
				test: /\.scss$/,
				use: [ 'style-loader', 'css-loader', 'sass-loader' ]
			}			
		]
	},
	resolve: {
		modules: [
			"node_modules",
			path.resolve(__dirname, './client')
		],
		extensions: ['.jsx', '.js', '.tsx', '.ts']
	},
	// TS Checker in a separate thread: TODO: replace with the NON-FORK version as soon as it is available
	plugins: [
    new webpack.ProvidePlugin({
      THREE: 'three'
    }),
		new ForkTsCheckerWebpackPlugin(),
    ]
};
