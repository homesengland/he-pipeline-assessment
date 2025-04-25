const gulp = require('gulp');
const concat = require('gulp-concat');

// Task to concatenate CSS files from monaco-editor
gulp.task('concat-css', function () {
    return gulp.src('node_modules/monaco-editor/**/*.css') // Adjust the path if needed
        .pipe(concat('monaco-styles.css'))
        .pipe(gulp.dest('src/assets/css')); // Output directory
});
