var myApp = angular.module('App', []);
myApp.controller('calculatorController', ['$scope', '$http', function EventListController($scope, $http) {

    $scope.loading = true;
    $scope.addMode = true;
    $scope.results = "";
    $scope.serviceURL = "http://localhost:52536";

    //Used to display the data 
    $http.get($scope.serviceURL + '/api/Calculator/').then(function (data) {
        $scope.Calculations = data;
        $scope.loading = false;
        $scope.error = "";
    });

    $scope.toggleEdit = function () {
        this.Calculator.editMode = !this.Calculator.editMode;
    };
    $scope.toggleAdd = function () {
        $scope.addMode = !$scope.addMode;
    };

    //Used to save a record after edit 
    $scope.save = function () {
        alert("Edit");
        $scope.loading = true;
        var calculation = this.Calculator;
        $http.put($scope.serviceURL + '/api/Calculator/', calculation).then(function (data) {
            alert("Saved Successfully!!");
            $scope.loading = false;
        });
    };

    //Used to calculate some data
    $scope.calculateData = function () {
        $scope.loading = true;
        var calculation = this.Calculator;
        $http({
            url: $scope.serviceURL + '/api/Calculator/',
            method: "GET",
            params: { fullExpression: $scope.calcExpression }
        }).then(function (data) {
            $scope.results = data.data;
            $scope.loading = false;
            $scope.error = "";
        });
    };

    //Used to add a new record 
    $scope.add = function () {
        $scope.loading = true;
        $http.post($scope.serviceURL + '/api/Calculator/', this.newCalculation).then(function (data) {
            alert("Added Successfully!!");
            $scope.addMode = false;
            $scope.Calculators.push(data);
            $scope.loading = false;
        });
    };

    //Used to edit a record 
    $scope.delete = function () {
        $scope.loading = true;
        var CalculatorId = this.Calculator.Id;
        $http.delete($scope.serviceURL + '/api/Calculator/' + CalculatorId).then(function (data) {
            alert("Deleted Successfully!!");
            $.each($scope.Calculations, function (i) {
                if ($scope.Calculations[i].Id === CalculatorId) {
                    $scope.Calculations.splice(i, 1);
                    return false;
                }
            });
            $scope.loading = false;
        });
    };

}]);