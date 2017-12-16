function calculatorController($scope, $http) {
    $scope.loading = true;
    $scope.addMode = true;
    $scope.calcExpression = "";
    $scope.results = "";

    //Used to display the data 
    $http.get('/api/Calculator/').success(function (data) {
        $scope.Calculations = data;
        $scope.loading = false;
        $scope.error = "";
    })
    .error(function () {
        $scope.error = "An Error has occured while loading calculations!";
        $scope.loading = false;
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
        alert(emp);
        $http.put('/api/Calculator/', calculation).success(function (data) {
            alert("Saved Successfully!!");
            emp.editMode = false;
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Calculation! " + data;
            $scope.loading = false;

        });
    };

    //Used to calculate some data
    $scope.calculateData = function () {
        alert("Calculating");
        $scope.loading = true;
        var calculation = this.Calculator;
        alert(emp);
        $http({
            url: user.details_path, 
            method: "GET",
            params: {fullExpression: $scope.calcExpression}
        }).success(function (data) {
            $scope.results = data;
            $scope.loading = false;
            $scope.error = "";
        })
         .error(function () {
             $scope.error = "An Error has occured while calculating data!";
             $scope.loading = false;
         });
    };

    //Used to add a new record 
    $scope.add = function () {
        $scope.loading = true;
        $http.post('/api/Calculator/', this.newCalculation).success(function (data) {
            alert("Added Successfully!!");
            $scope.addMode = false;
            $scope.Calculators.push(data);
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Adding Calculation! " + data;
            $scope.loading = false;

        });
    };

    //Used to edit a record 
    $scope.deleteContact = function () {
        $scope.loading = true;
        var CalculatorId = this.Calculator.Id;
        $http.delete('/api/Calculator/' + CalculatorId).success(function (data) {
            alert("Deleted Successfully!!");
            $.each($scope.Calculations, function (i) {
                if ($scope.Calculations[i].Id === CalculatorId) {
                    $scope.Calculations.splice(i, 1);
                    return false;
                }
            });
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Contact! " + data;
            $scope.loading = false;

        });
    };

}