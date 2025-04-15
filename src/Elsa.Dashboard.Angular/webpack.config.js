const MonacoWebpackPlugin = require('monaco-editor-webpack-plugin');
const path = require('path');
const AngularWebpackPlugin = require('@ngtools/webpack').AngularWebpackPlugin;

module.exports = {
	entry: './src/main.tsx',
	output: {
		path: path.resolve(__dirname, 'dist'),
		filename: 'app.js'
	},
	resolve: {
		extensions: ['.ts', '.js']
	},
	module: {
		rules: [
			{
				test: /\.css$/,
				use: ['style-loader', 'css-loader', 'postcss-loader']
			},
			{
				test: /\.ttf$/,
				type: 'asset/resource'
			},
			{
				test: /\.ts$/,
				loader: '@ngtools/webpack'
			}
		]
	},
	plugins: [
		new MonacoWebpackPlugin(),
		new AngularWebpackPlugin({
			tsConfigPath: './tsconfig.json',
			entryModule: './src/app/app.module.ts',
			sourceMap: true
		})
	]
};
