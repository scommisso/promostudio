{

    //By default, all modules are located relative to this path. If baseUrl
    //is not explicitly set, then all modules are loaded relative to
    //the directory that holds the build file. If appDir is set, then
    //baseUrl should be specified as relative to the appDir.
    baseUrl: './Scripts',

    //By default all the configuration for optimization happens from the command
    //line or by properties in the config file, and configuration that was
    //passed to requirejs as part of the app's runtime "main" JS file is *not*
    //considered. However, if you prefer the "main" JS file configuration
    //to be read for the build so that you do not have to duplicate the values
    //in a separate configuration, set this property to the location of that
    //main JS file. The first requirejs({}), require({}), requirejs.config({}),
    //or require.config({}) call found in that file will be used.
    mainConfigFile: './Scripts/requireConfig.js',

    //The directory path to save the output. If not specified, then
    //the path will default to be a directory called "build" as a sibling
    //to the build file. All relative paths are relative to the build file.
    dir: "optimizedScripts",

    //How to optimize all the JS files in the build output directory.
    //Right now only the following values
    //are supported:
    //- "uglify": (default) uses UglifyJS to minify the code.
    //- "uglify2": in version 2.1.2+. Uses UglifyJS2.
    //- "closure": uses Google's Closure Compiler in simple optimization
    //mode to minify the code. Only available if running the optimizer using
    //Java.
    //- "closure.keepLines": Same as closure option, but keeps line returns
    //in the minified files.
    //- "none": no minification will be done.
    optimize: "none",//"uglify",

    //Allow CSS optimizations. Allowed values:
    //- "standard": @import inlining, comment removal and line returns.
    //Removing line returns may have problems in IE, depending on the type
    //of CSS.
    //- "standard.keepLines": like "standard" but keeps line returns.
    //- "none": skip CSS optimizations.
    //- "standard.keepComments": keeps the file comments, but removes line
    //returns.  (r.js 1.0.8+)
    //- "standard.keepComments.keepLines": keeps the file comments and line
    //returns. (r.js 1.0.8+)
    optimizeCss: "none",

    //If optimizeCss is in use, a list of files to ignore for the @import
    //inlining. The value of this option should be a string of comma separated
    //CSS file names to ignore (like 'a.css,b.css'. The file names should match
    //whatever strings are used in the @import calls.
    cssImportIgnore: null,

    //Inlines the text for any text! dependencies, to avoid the separate
    //async XMLHttpRequest calls to load those dependencies.
    inlineText: true,

    //List the modules that will be optimized. All their immediate and deep
    //dependencies will be included in the module's file when the build is
    //done. If that module or any of its dependencies includes i18n bundles,
    //only the root bundles will be included unless the locale: section is set above.
    
    paths: {
        "modernizr": "empty:",
        "jquery": "empty:",
        "jqueryui": "empty:",
        "knockout": "empty:",
        "bootstrap": "empty:",
        "bootstrap-lightbox": "empty:",
        "form": "empty:",
        "strings": "empty:",
        "facebookOAuth": "empty:",
        "googleOAuth": "empty:",
        "twitterOAuth": "empty:"
    },

    modules: [{
        name: "viewModels/scaffold"
    }, {
        name: "viewModels/loginViewModel"
    }, {
        name: "viewModels/buildNavViewModel"
    }, {
        name: "viewModels/footageViewModel"
    }, {
        name: "viewModels/brandingViewModel"
    }]
}