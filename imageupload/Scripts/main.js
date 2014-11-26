requirejs.config({
    baseUrl: 'Scripts',
    paths: {
        app: 'app',
        "selectmap" : "./modules/selectmap"
    }
});
// Start loading the main app file. Put all of
// your application logic in there.
requirejs(['app/main']);