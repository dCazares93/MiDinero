

sabio.ng.app.module.config(function ($routeProvider, $locationProvider) {

    $routeProvider.when('/', {
        templateUrl: '/Scripts/app/plans/index.html',
    })
    .when('/type/:typeId/view', {
        templateUrl: '/Scripts/app/plans/typeDetails.html',
        controller: 'plansDashboardTypesController',
        controllerAs: 'pc',
        reloadOnSearch: false
    })
    .when('/type/:typeId/edit', {
        templateUrl: '/Scripts/app/plans/typeForm.html',
        controller: 'plansDashboardTypesController',
        controllerAs: 'pc',
        reloadOnSearch: false
    })
    .when('/type/create', {
        templateUrl: '/Scripts/app/plans/typeForm.html',
        controller: 'plansDashboardTypesController',
        controllerAs: 'pc',
        reloadOnSearch: false
    })
    .when('/type/:typeId/plan/:planId/view', {
        templateUrl: '/Scripts/app/plans/planDetails.html',
        controller: 'plansDashboardPlansController',
        controllerAs: 'pc',
        reloadOnSearch: false
    })
    .when('/type/:typeId/plan/:planId/edit', {
        templateUrl: '/Scripts/app/plans/planForm.html',
        controller: 'plansDashboardPlansController',
        controllerAs: 'pc',
        reloadOnSearch: false
    })
    .when('/type/:typeId/plan/create', {
        templateUrl: '/Scripts/app/plans/planForm.html',
        controller: 'plansDashboardPlansController',
        controllerAs: 'pc',
        reloadOnSearch: false
    })
    .when('/type/:typeId/plan/:planId/task/:featureId/view', {
        templateUrl: '/Scripts/app/plans/featureDetails.html',
        controller: 'plansDashboardFeaturesController',
        controllerAs: 'pc',
        reloadOnSearch: false
    })
    .when('/type/:typeId/plan/:planId/task/:featureId/edit', {
        templateUrl: '/Scripts/app/plans/featureForm.html',
        controller: 'plansDashboardFeaturesController',
        controllerAs: 'pc',
        reloadOnSearch: false
    })
    .when('/type/:typeId/plan/:planId/task/create', {
        templateUrl: '/Scripts/app/plans/featureForm.html',
        controller: 'plansDashboardFeaturesController',
        controllerAs: 'pc',
        reloadOnSearch: false
    })
    .otherwise({
        redirectTo: '/'
    });

    $locationProvider.html5Mode({
        enabled: false
    });
});