
/*

 This script holds the controllers and logic for the admin plans UI Dashboard page.

 The plans are organized in a way where there are "Plan Types", which all possess Plans. Those Plans each have a "Features".
 "Features" is now being referred to as "Tasks" on the front-end. Any code to do with "Features" referrs to the Plan "Tasks"
 Ramona has planned for individual plans. Features == Tasks......

 The controllers and logic have been broken up to handle code relating to the Plan Types, Plans, Features, and the accordion that presents them on the page.
 

 */



//Features
sabio.page.plansDashboardFeaturesControllerFactory = function (
    $scope  //  these three arguments are dependencies which are specified below and passed into the controller by ng
    , $baseController
    , $routeParams
    , $route
    , $location
    , $plansService
    , $eventServiceFactory) {

    var vm = this;

    console.log("route", $route);
    console.log("rooute params", $routeParams);

    $baseController.merge(vm, $baseController);
    vm.$plansService = $plansService;
    vm.$routeParams = $routeParams;
    vm.$location = $location;
    vm.$eventServiceFactory = $eventServiceFactory
    vm.notify = vm.$plansService.getNotifier($scope);

    vm.typeId = ($routeParams.typeId) ? $routeParams.typeId : null;
    vm.planId = ($routeParams.planId) ? $routeParams.planId : null;
    vm.featureId = ($routeParams.featureId) ? $routeParams.featureId : null;
    vm.featureForm = null;
    vm.featureFormData = null;
    vm.priorityLevels = [];
    vm.formErrors = false;
    vm.header = "Task Form";
    vm.saveFeature = _saveFeature;
    vm.deleteFeature = _deleteFeature;

    _init();

    function _init() {
        vm.header = (vm.featureId) ? "Edit Task #" + vm.featureId : "Create Task";
        vm.deleteButton = (vm.featureId) ? true : false;

        if (vm.featureId) {
            vm.$plansService.getById(vm.featureId, onGetFeatureSuccess, onError)
        }
        else {
            getPriorityLevels();
        }

    }

    function onGetFeatureSuccess(data) {
        vm.notify(function () {
            vm.featureFormData = data.item;
            vm.featureFormData.dateAdded = new Date(data.item.dateAdded).toLocaleString();
            vm.featureFormData.dateModified = new Date(data.item.dateModified).toLocaleString();
            getPriorityLevels();
        });
    }


    function getPriorityLevels() {
        vm.$plansService.getPriorityLevels(onGetPriorityLevelsSuccess, onError);
    }

    function onGetPriorityLevelsSuccess(data) {
        vm.notify(function () {
            vm.priorityLevels = data.items;
        });
    }


    function _saveFeature() {
        vm.formErrors = true;
        if (vm.featureForm.$valid) {
            vm.formErrors = false;
            if (vm.featureId) {
                console.log("update data for plan #%s", vm.planId, vm.featureId);
                var data = vm.featureFormData;
                data.id = vm.featureId
                data.planId = vm.planId;
                data.category = vm.typeId;
                data.sortOrder = data.sortOrder.id
                console.log('form is valid', data);
                vm.$plansService.update(vm.planId, vm.featureId, data, onFeatureUpdateSuccess, onError);
            }
            else {
                console.log("create new plan");
                var data = vm.featureFormData;
                data.planId = vm.planId;
                data.category = vm.typeId;
                data.sortOrder = data.sortOrder.id
                console.log('form is valid', data);
                vm.$plansService.add(vm.planId, data, onAddFeatureSuccess, onError);
            }
        }
        else {
            console.error("form is invalid");
        }
    }

    function onAddFeatureSuccess() {
        sabio.ui.alerts.notifications.success("Task Added!");
        console.log("success on adding");
        vm.$eventServiceFactory.broadcast('FeaturesRefresh', vm.planId);
        //window.setTimeout(dashboard, 45);
    }

    function onFeatureUpdateSuccess() {
        sabio.ui.alerts.notifications.success("Task Updated!");
        console.log("success on updating");
        vm.$eventServiceFactory.broadcast('FeaturesRefresh', vm.planId);
        //window.setTimeout(dashboard, 45);
    }

    //function dashboard() {
    //    vm.$location.url("/");
    //}

    function onError(jqXhr, error) {
        sabio.ui.alerts.notifications.danger("There was an error! Try again");
        console.log(error);
    }


    function _deleteFeature() {
        var msg = "Are you sure you want to delete this Task? This cannot be undone.";
        sabio.ui.alerts.confirm(msg, onConfirm, onNo);
    }

    function onConfirm() {
        vm.$plansService.delete(vm.featureId, onDeleteFeatureSuccess, onError);
    }

    function onDeleteFeatureSuccess() {
        vm.$eventServiceFactory.broadcast('FeaturesRefresh', vm.planId);
        window.setTimeout(dashboard, 45);
    }

    function onNo() {
        console.log("Feature not deleted!");
    }

}

sabio.ng.addController(sabio.ng.app.module
    , "plansDashboardFeaturesController"
    , ['$scope', '$baseController', '$routeParams', '$route', '$location', '$plansService', '$eventServiceFactory']
    , sabio.page.plansDashboardFeaturesControllerFactory);



//Plans
sabio.page.plansDashboardPlansControllerFactory = function (
    $scope
    , $baseController
    , $routeParams
    , $route
    , $location
    , $plansService
    , $eventServiceFactory) {

    var vm = this;

    console.log("route", $route);
    console.log("rooute params", $routeParams);

    $baseController.merge(vm, $baseController);
    vm.$plansService = $plansService;
    vm.$routeParams = $routeParams;
    vm.$location = $location;
    vm.$eventServiceFactory = $eventServiceFactory
    vm.notify = vm.$plansService.getNotifier($scope);

    vm.planId = ($routeParams.planId) ? $routeParams.planId : null;
    vm.typeId = ($routeParams.typeId) ? $routeParams.typeId : null;
    vm.planForm = null;
    vm.planFormData = null;
    vm.types = [];
    vm.formErrors = false;
    vm.header = "Plan Form";
    vm.savePlan = _savePlan;
    vm.deletePlan = _deletePlan;

    _init();

    function _init() {
        //console.log("types init, mode=%s", (vm.planId) ? "EDIT" : "CREATE");
        vm.header = (vm.planId) ? "Edit Plan #" + vm.planId : "Create Plan";
        vm.deleteButton = (vm.planId) ? true : false;
        vm.typesDropDown = (vm.typeId && vm.planId) ? true : false;
        vm.planFormData = (vm.planId) ?
            vm.$plansService.getPlanById(vm.planId, onGetPlanSuccess, onError) : null
    }


    function getPlanTypes() {
        vm.$plansService.getPlanTypes(onGetPlanTypesSuccess, onError);
    }

    function onGetPlanTypesSuccess(data) {
        vm.notify(function () {
            vm.types = data.items;
        });
    }

    function onGetPlanSuccess(data) {
        vm.notify(function () {
            vm.planFormData = data.item;
            vm.planFormData.goLiveDate = new Date(data.item.goLiveDate).toLocaleDateString();
            vm.planFormData.dateAdded = new Date(data.item.dateAdded).toLocaleString();
            vm.planFormData.dateModified = new Date(data.item.dateModified).toLocaleString();
            getPlanTypes();
        });
    }


    function _savePlan() {
        vm.formErrors = true;
        if (vm.planForm.$valid) {
            vm.formErrors = false;
            if (vm.planId) {
                var data = vm.planFormData;
                if (vm.planFormData.tag) {
                    vm.planFormData.tag = vm.planFormData.tag.replace(/ /g, '');
                }
                data.types = data.type.id;
                data.id = vm.planId;
                vm.$plansService.updatePlan(vm.planId, data, onPlanUpdateSuccess, onError);
            }
            else {
                var data = vm.planFormData;
                if (vm.planFormData.tag) {
                    vm.planFormData.tag = vm.planFormData.tag.replace(/ /g, '');
                }

                data.types = vm.typeId;
                console.log('data to ajax addplan');
                console.log(data);
                vm.$plansService.addPlan(data, onAddPlanSuccess, onError)
            }
        }
        else {
            console.error("form is invalid");
        }
    }


    function onAddPlanSuccess() {
        sabio.ui.alerts.notifications.success("Plan Added!");
        //console.log("success on adding");
        vm.$eventServiceFactory.broadcast('PlansRefresh', vm.typeId);
        //window.setTimeout(dashboard, 45);
    }

    function onPlanUpdateSuccess() {
        sabio.ui.alerts.notifications.success("Plan Updated!");
        //console.log("success on updating");
        vm.$eventServiceFactory.broadcast('PlansRefresh', vm.typeId);
        //window.setTimeout(dashboard, 45);
    }

    function dashboard() {
        vm.$location.url("/");
    }

    function onError(jqXhr, error) {
        sabio.ui.alerts.notifications.danger("There was an error! Try again");
        console.log(error);
    }


    function _deletePlan() {
        var msg = "Are you sure you want to delete this plan? You will also delete all tasks included. This cannot be undone.";
        sabio.ui.alerts.confirm(msg, onConfirm, onNo);
    }

    function onConfirm() {
        vm.$plansService.deletePlanWithFeatures(vm.planId, onDeletePlanSuccess, onError);
    }

    function onDeletePlanSuccess() {
        vm.$eventServiceFactory.broadcast('PlansRefresh', vm.typeId);
        //window.setTimeout(dashboard, 45);
    }

    function onNo() {
        console.log("Plan not deleted!");
    }

}

sabio.ng.addController(sabio.ng.app.module
    , "plansDashboardPlansController"
    , ['$scope', '$baseController', '$routeParams', '$route', '$location', '$plansService', '$eventServiceFactory']
    , sabio.page.plansDashboardPlansControllerFactory);



//PlanTypes
sabio.page.plansDashboardTypesControllerFactory = function (
    $scope  //  these three arguments are dependencies which are specified below and passed into the controller by ng
    , $baseController
    , $routeParams
    , $route
    , $location
    , $plansService
    , $eventServiceFactory) {

    var vm = this;

    console.log("route", $route);
    console.log("route params", $routeParams);

    $baseController.merge(vm, $baseController);
    vm.$plansService = $plansService;
    vm.$routeParams = $routeParams;
    vm.$location = $location;
    vm.$eventServiceFactory = $eventServiceFactory
    vm.notify = vm.$plansService.getNotifier($scope);

    vm.typeId = ($routeParams.typeId) ? $routeParams.typeId : null;
    vm.planTypesForm = null;
    vm.planTypesFormData = null;
    vm.formErrors = false;
    vm.header = "Plan Type";
    vm.saveType = _saveType;
    vm.deleteType = _deleteType;

    _init();

    function _init() {
        console.log("types init, mode=%s", (vm.typeId) ? "EDIT" : "CREATE");
        vm.header = (vm.typeId) ? "Edit Plan Type #" + vm.typeId : "Create Plan Type";
        vm.deleteButton = (vm.typeId) ? true : false;
        vm.planTypesFormData = (vm.typeId) ?
            vm.$plansService.getPlanTypesById(vm.typeId, onGetPlanTypeSuccess, onError) : null
    }

    function onGetPlanTypeSuccess(data) {
        vm.notify(function () {
            vm.planTypesFormData = data.item;
        });
    }


    function _saveType() {
        vm.formErrors = true;
        if (vm.planTypesForm.$valid) {
            vm.formErrors = false;
            if (vm.typeId) {
                console.log("update data for type #%s", vm.typeId);
                var data = vm.planTypesFormData;
                data.id = vm.typeId;
                console.log("form is valid", vm.planTypesFormData);
                vm.$plansService.updatePlanType(vm.typeId, vm.planTypesFormData, onUpdatePlanTypeSuccess, onError)

            }
            else {
                console.log("create new type");
                console.log("form is valid", vm.planTypesFormData);
                vm.$plansService.addPlanType(vm.planTypesFormData, onAddPlanTypeSuccess, onError)
            }
        }
        else {
            console.error("form is invalid");
        }
    }

    function onAddPlanTypeSuccess() {
        sabio.ui.alerts.notifications.success("Plan Type Added!");
        console.log("success on adding");
        vm.$eventServiceFactory.broadcast('TypesRefresh');
        //window.setTimeout(dashboard, 45);
    }

    function onUpdatePlanTypeSuccess() {
        sabio.ui.alerts.notifications.success("Plan Type Updated!");
        console.log("success on updating");
        vm.$eventServiceFactory.broadcast('TypesRefresh');
        //window.setTimeout(dashboard, 45);
    }

    //function dashboard() {
    //    vm.$location.url("/");
    //}

    function onError(jqXhr, error) {
        sabio.ui.alerts.notifications.danger("There was an error! Try again");
        console.log(error);
    }


    function _deleteType() {
        var msg = "Are you sure you want to delete this plan type? You will also delete all plans and tasks included. This cannot be undone.";
        sabio.ui.alerts.confirm(msg, onConfirm, onNo);
    }

    function onConfirm() {
        vm.$plansService.deletePlanType(vm.typeId, onDeleteTypeSuccess, onError);
    }

    function onDeleteTypeSuccess() {
        vm.$eventServiceFactory.broadcast('TypesRefresh');
        //window.setTimeout(dashboard, 45);
    }

    function onNo() {
        console.log("Type not deleted!");
    }

}

sabio.ng.addController(sabio.ng.app.module
    , "plansDashboardTypesController"
    , ['$scope', '$baseController', '$routeParams', '$route', '$location', '$plansService', '$eventServiceFactory']
    , sabio.page.plansDashboardTypesControllerFactory);



//Accordion
sabio.page.plansAccordionControllerFactory = function (
    $scope
    , $baseController
    , $plansService
    , $location
    , $log
    , $eventServiceFactory) {

    var vm = this;

    console.log("event factory", $eventServiceFactory);
    $baseController.merge(vm, $baseController);
    vm.$scope = $scope;
    vm.$plansService = $plansService;
    vm.notify = vm.$plansService.getNotifier($scope);
    vm.$location = $location;
    vm.$log = $log;
    vm.$eventServiceFactory = $eventServiceFactory;


    //  hoist public API
    vm.loadPlansTab = _loadPlansTab;
    vm.loadFeaturesTab = _loadFeaturesTab;
    vm.loadPlansByType = _loadPlansByType;

    //  properties to enable our business logic
    vm.planTypes = null;
    vm.selectedPlanType = null;
    vm.selectedPlan = null;
    vm.plansCache = {};
    vm.featuresCache = {};
    vm.plans = null;
    vm.features = null;
    vm.panelHeader = "Manage Plans";
    vm.planRefreshListener = _planRefreshListener;
    vm.refreshPlans = _refreshPlans;
    vm.featureRefreshListener = _featureRefreshListener
    vm.refreshFeatures = _refreshFeatures;


    _init();

    vm.$eventServiceFactory.listen('TypesRefresh', _init);


    vm.$eventServiceFactory.listen('PlansRefresh', vm.planRefreshListener);

    function _planRefreshListener(context, data) {
        vm.refreshPlans(data);
    }

    function _refreshPlans(typeId) {
        vm.$plansService.getPlansByType(typeId, _onGetPlansSuccess, _onGetPlansError);
    }


    vm.$eventServiceFactory.listen('FeaturesRefresh', vm.featureRefreshListener);

    function _featureRefreshListener(context, data) {
        vm.refreshFeatures(data);
    }

    function _refreshFeatures(planId) {
        vm.$plansService.getByPlanId(planId, _onGetFeaturesSuccess, _onGetFeaturesError);
    }



    function _init() {
        vm.$plansService.getPlanTypes(_onGetPlanTypesSuccess, _onGetPlanTypesError);
    }

    function _onGetPlanTypesSuccess(data) {
        vm.notify(function () {
            vm.planTypes = data.items;
        });
    }

    function _onGetPlanTypesError(error) {
        console.error("problem getting plan types", error);
    }


    function _loadPlansTab(planType) {
        if (vm.selectedPlanType == planType) {
            vm.selectedPlanType = null;
        }
        else {
            vm.selectedPlanType = planType;
            _loadPlansByType(planType.id);
        }
    }

    function _loadPlansByType(planTypeId) {
        if (vm.plansCache[planTypeId]) {
            console.log("plans cache hit for type %s", planTypeId);
            vm.plans = vm.plansCache[planTypeId];

        }
        else {
            console.log("plans cache miss -> load plans from server for type %s", planTypeId);
            vm.$plansService.getPlansByType(planTypeId, _onGetPlansSuccess, _onGetPlansError);

        }
    }

    function _onGetPlansSuccess(data) {
        if (vm.selectedPlanType) {
            vm.plansCache[vm.selectedPlanType.id] = data.items;

            vm.notify(function () {
                vm.plans = vm.plansCache[vm.selectedPlanType.id];

            });
        }
    }

    function _onGetPlansError(error) {
        console.error("problem getting plans", error);
    }


    function _loadFeaturesTab(plan) {
        if (vm.selectedPlan == plan) {
            vm.selectedPlan = null;
        }
        else {
            vm.selectedPlan = plan;
            _loadFeaturesByPlan(plan.id);
        }
    }

    function _loadFeaturesByPlan(planId) {
        if (vm.featuresCache[planId]) {
            console.log("feature cache hit for plan %s", planId);
            vm.features = vm.featuresCache[planId];
        }
        else {
            console.log("features cache miss -> load features from server for plan %s", planId);
            vm.$plansService.getByPlanId(planId, _onGetFeaturesSuccess, _onGetFeaturesError);
        }
    }

    function _onGetFeaturesSuccess(data) {
        vm.featuresCache[vm.selectedPlan.id] = data.items;

        vm.notify(function () {
            vm.features = vm.featuresCache[vm.selectedPlan.id];
        });
    }

    function _onGetFeaturesError(error) {
        console.error("problem getting features", error);
    }

}

sabio.ng.addController(sabio.ng.app.module
    , "plansAccordionController"
    , ['$scope', '$baseController', '$plansService', '$location', '$log', '$eventServiceFactory']
    , sabio.page.plansAccordionControllerFactory);