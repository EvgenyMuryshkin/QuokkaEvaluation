var path = require('path');
var webpack = require('webpack');

var ForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin');

var BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

module.exports = {
	stats: "errors-only",
	watchOptions: {	
		ignored: /node_modules/
	},
	//devtool: "eval-source-map",
	entry: {
		platform: ["client", "./client/boot-client.tsx"],
	},
	output: {
		path: path.resolve(__dirname,"./wwwroot/dist"),
		filename: "[name].js"
	},
	module: {
		rules: [
			{
				test: /\.tsx?$/,
				include: [path.resolve(__dirname, "src")],
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
			path.resolve(__dirname, './src')
		],
		extensions: ['.jsx', '.js', '.tsx', '.ts']
	},
	// TS Checker in a separate thread: TODO: replace with the NON-FORK version as soon as it is available
	plugins: [
		new ForkTsCheckerWebpackPlugin(),
		//new BundleAnalyzerPlugin({openAnalyzer: false}),
    ]
};
