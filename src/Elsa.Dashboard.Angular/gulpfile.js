const gulp = require('gulp');
const concat = require('gulp-concat');
const merge = require('merge-stream');

// Task to concatenate CSS files from monaco-editor
gulp.task('concat-css', function () {
    return gulp.src('node_modules/monaco-editor/**/*.css') // Adjust the path if needed
        .pipe(concat('monaco-styles.css'))
        .pipe(gulp.dest('src/assets/css')); // Output directory
});

gulp.task('concat-monaco', function (){
  // Create a stream for JS files
  const jsFiles = gulp.src('node_modules/monaco-editor/min/**/*.js')
    .pipe(gulp.dest('src/assets/monaco-editor/min'));
  
  // Create a stream for font files
  const fontFiles = gulp.src('node_modules/monaco-editor/min/**/*.ttf')
    .pipe(gulp.dest('src/assets/monaco-editor/min'));
  
  // Return a merged stream to handle both operations
  return merge(jsFiles, fontFiles);
})