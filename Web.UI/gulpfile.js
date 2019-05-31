/// <reference path="content/assets_src/js/vendor/jquery.maskmoney.js" />
/// <reference path="content/assets_src/js/vendor/jquery.maskmoney.js" />
/*
|--------------------------------------------------------------------------
| Gulpfile
|--------------------------------------------------------------------------
*/

var gulp = require('gulp');
var fs = require('fs');
var pkg = JSON.parse(fs.readFileSync('./package.json'));

// Plugins
var sourcemaps = require('gulp-sourcemaps');
var watch = require('gulp-watch');
var outputJs = './Content/assets/js';
var outputCss = './Content/assets/css'
' */\n\n'

//Plugins News
var plumber = require('gulp-plumber');
var clean = require('gulp-clean');
var usemin = require('gulp-usemin');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var autoprefixer = require('gulp-autoprefixer');
var cssmin = require('gulp-cssmin');
var csscomb = require('gulp-csscomb');
var htmlReplace = require('gulp-html-replace');
var sass = require('gulp-sass');
var imagemin = require('gulp-imagemin');
var runSequence = require('run-sequence');
var cache = require('gulp-cache');
var jshint = require('gulp-jshint');
var jshintStylish = require('jshint-stylish');
var csslint = require('gulp-csslint');
var babel = require('gulp-babel');
var header = require('gulp-header');

var path = require('path');
var browserSync = require('browser-sync').create();
var msbuild = require("gulp-msbuild");
var iisexpress = require('gulp-serve-iis-express');

var PORT = '54907';
/*
|--------------------------------------------------------------------------
| Sources CS
|--------------------------------------------------------------------------
*/

var sources = [
    'Controllers/*.cs',
    'Helpers/*.cs',
    'ViewModel/**/*.cs'
];
var views = [
    'Views/**/*.cshtml',
];

/*
|--------------------------------------------------------------------------
| Paths
|--------------------------------------------------------------------------
*/
var paths = {
    
        sass: [
                'content/assets_src/sass/*.scss',
                'content/assets_src/sass/style.scss'
        ],
        sass_lint: [
                //'content/assets_src/sass/**/*.scss',
        ],
        js_lint: [
                'content/assets_src/js/**/*.js'
        ],
        css_lint: [
                //'content/assets_src/css/**/*.css'
        ],
        js: [
                //jquery 
                'content/assets_src/js/vendor/jquery-1.12.4.min.js',
                'content/assets_src/js/vendor/jquery-ui-1.12.1.min.js',
                'content/assets_src/js/vendor/jquery.serializeToJSON.min.js',
                
                ////jquery.validate
                'content/assets_src/js/vendor/jquery.validate.min.js',
                //'content/assets_src/js/vendor/jquery.validate.additional-methods.js',

                ////DataTables
                'content/assets_src/js/vendor/datatables.min.js',
                'content/assets_src/js/vendor/DataTables/dataTables.bootstrap.min.js',
                'content/assets_src/js/vendor/DataTables/dataTables.responsive.js',
                'content/assets_src/js/vendor/DataTables/dataTables.buttons.min.js',
                'content/assets_src/js/vendor/lobipanel.min.js',

                ////Bootstrap
                'content/assets_src/js/vendor/bootstrap.min.js',
                ////Bootbox
                'content/assets_src/js/vendor/bootbox.min.js',

                ////Outros
                'content/assets_src/js/vendor/jquery.mask.min.js',
                'content/assets_src/js/vendor/jquery.g2it.mascaras-pt-BR.js',
                //'content/assets_src/js/vendor/jquery.datetimepicker.full.min.js',
                'content/assets_src/js/vendor/jquery.timepicker.min.js',
                'content/assets_src/js/vendor/jquery.barrating.min.js',

                ////Admin
                'content/assets_src/js/vendor/admin.js',

                ////System
                'content/assets_src/js/core/_nameSpace.js',
                'content/assets_src/js/core/_core.js',
                'content/assets_src/js/model/*.js',
                'content/assets_src/js/components/*.js',
                'content/assets_src/js/controller/*.js',
        ],
        css: [
                'content/assets_src/css/*.css'
        ],
        outputCss: [
                'content/assets/css'
        ],
        outputJs: [
                'content/assets/js'
        ]
    };

/*
|--------------------------------------------------------------------------
| Tarefas Gerais
|--------------------------------------------------------------------------
*/

//Default
gulp.task('default', function () {
    runSequence('sass', ['js-copy', 'css-copy'],
		['watch']);
});

//Build
gulp.task('build', function () {
    runSequence('clean:assets', 'clear:cache', 'sass',
		['concat-js', 'concat-css', 'fonts', 'optimize-img']);
});

/*
|--------------------------------------------------------------------------
| Start Servidor CS
|--------------------------------------------------------------------------
*/
//gulp.task('startcs', ['server', 'buildVS'], function() {
//    browserSync.init({
//        baseDir: 'content',
//        proxy: 'http://localhost:' + PORT,
//        notify: false,
//        ui: false
//    });
//    gulp.watch(sources, ['buildVS']);
//    return gulp.watch(views, ['reload']);
//});

//gulp.task('reload', function() {
//    browserSync.reload();
//});

//gulp.task('buildVS', function() {
//    return gulp.src("../e-Qualit.sln")
//        .pipe(plumber())
//        .pipe(msbuild({
//            toolsVersion: 'auto',
//            logCommand: true
//        }));
//});

//gulp.task('server', function() {
//    var configPath = path.join(__dirname, '..\\.vs\\config\\applicationhost.config');
//    iisexpress({
//        siteNames: ['Web.UI'],
//        configFile: configPath,
//        port: PORT
//    });
//});

//Task de Watch (browserSync / Watch)
gulp.task('watch', function () {

    gulp.watch('content/**/*').on('change', browserSync.reload);

    gulp.watch(paths.js_lint, ['js-copy']).on('change', function (event) {
        console.log("Linting JS - " + event.path);
        gulp.src(event.path)
            .pipe(jshint({'esversion': 6}))
            .pipe(jshint.reporter(jshintStylish));
    });

    //Lint CSS
    gulp.watch(paths.css_lint, ['sass']).on('change', function (event) {
        console.log("Linting CSS - " + event.path);
        gulp.src(event.path)
            .pipe(csslint())
            .pipe(csslint.formatter());
    });

    //Compila SASS
    gulp.watch(paths.sass_lint, ['sass']).on('change', function (event) {
        console.log('Compilando SASS - ' + event.path);
        gulp.src(event.path)
 			.pipe(sass().on('error', function (error) {
 			    console.log('Problema na compilação');
 			    console.log(error.message);
 			}))
 			.pipe(gulp.dest('content/assets_src/css'));
    });

});

/*
|--------------------------------------------------------------------------
| Tarefas para Desenvolvimento
|--------------------------------------------------------------------------
*/

// Sass
gulp.task('sass', function () {
    return gulp
        .src(paths.sass)
		.pipe(sass())
        .pipe(autoprefixer())
        .pipe(csscomb())
        .pipe(gulp.dest('content/assets_src/css'));
});

//Copy JS
gulp.task('js-copy', function () {
    return gulp
		.src(paths.js)
		.pipe(plumber())
        .pipe(concat('scripts.js'))
		.pipe(gulp.dest(outputJs));
});

//Copy CSS
gulp.task('css-copy', function () {
    return gulp
		.src(paths.css)
		.pipe(plumber())
        .pipe(concat('style.css'))
		.pipe(gulp.dest(outputCss));
});

/*
|--------------------------------------------------------------------------
| Tarefas de Otimização (USEMIN)
|--------------------------------------------------------------------------
*/

//Concatena, Minifica, Replace HTML e coloca os prefixers dos arquivos JS e CSS
/* - Envolver os scripts nas TAGs utilizadas pelo usemin
<!-- build:css css/style.min.css -->
<!-- endbuild -->
<!-- build:js js/scripts.min.js -->
<!-- endbuild -->
*/
//gulp.task('usemin', ['sass', 'js-copy'], function () {
//    return gulp
//		.src('views/**/*.html')
//		.pipe(plumber())
//		.pipe(usemin({
//		    'js': [uglify],
//		    'css': [autoprefixer, cssmin]
//		}))
//		.pipe(gulp.dest('content/assets'));
//});

/*
|--------------------------------------------------------------------------
| Tarefas de Otimização (SEM USEMIN)
|--------------------------------------------------------------------------
*/

//Concatena e Minifica os arquivos JS
gulp.task('concat-js', function () {
    return gulp
		.src(paths.js)
        //.pipe(babel({ presets: ['es2015'] }))
        .pipe(jshint.reporter(jshintStylish))
        .pipe(plumber())
        .pipe(concat('scripts.js'))
        //.pipe(uglify())
        .pipe(gulp.dest(outputJs));
});

//Concatena e Minifica os arquivos CSS
gulp.task('concat-css', function () {
    return gulp
		.src(paths.css)
		.pipe(plumber())
        .pipe(concat('style.css'))
		.pipe(cssmin())
		.pipe(gulp.dest(outputCss));
});

//Replace no HTML dos arquivos concatenados
gulp.task('html-replace', function () {
    return gulp
		.src('content/view/Shared/_Layout.cshtml')
        .pipe(header('\ufeff'))
		.pipe(htmlReplace({
		    js: '../../content/assets/js/scripts.js',
		    css: '../../content/assets/css/style.css'
		}))
		.pipe(gulp.dest('views/Shared/'));
});

// Otimiza as Imagens
gulp.task('optimize-img', function () {
    return gulp
		.src('content/assets_src/imagens/**/*.+(png|jpg|gif|svg)')
		.pipe(cache(imagemin({
		    interlaced: true
		})))
		.pipe(gulp.dest('content/assets/imagens'));
});

/*
|--------------------------------------------------------------------------
| Tarefas Gerais
|--------------------------------------------------------------------------
*/

// Copia as Fonts
gulp.task('fonts', function () {
    return gulp
		.src('content/assets_src/fonts/**/*')
		.pipe(gulp.dest('content/assets/fonts'))
});


/*
|--------------------------------------------------------------------------
| Tarefas de Limpeza
|--------------------------------------------------------------------------
*/

// Clean assets folder
gulp.task('clean:assets', function () {
    return gulp.src('content/assets')
		.pipe(clean());
});

// Clear Cache
gulp.task('clear:cache', function () {
    return cache.clearAll();
});
