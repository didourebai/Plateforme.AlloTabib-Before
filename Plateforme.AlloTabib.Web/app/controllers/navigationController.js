'use strict';
app.controller('navigationController',
    function ($scope, $location, $window, authService, patientApiServices, $http, $rootScope) {

        var _authentication = {
            isAuth: false,
            userName: "",
            nomPrenom: ""
        };
     
        $scope.isActive = function (path) {
           
            return $location.path().substr(0, path.length) == path;
          
            //changer le css
        };

      
        $scope.authentication = $window.sessionStorage["userInfo"];
        $rootScope.userName = $window.sessionStorage["userName"];
        $rootScope.isAuth = $window.sessionStorage["isAuth"];
        $rootScope.nomPrenom = $window.sessionStorage["nomPrenom"];

        $scope.$watch('isAuth', function () {
            $rootScope.userName = $window.sessionStorage["userName"];
            $rootScope.nomPrenom = $window.sessionStorage["nomPrenom"];

        }, true);


        //$scope.$watch($scope.isAuth, function (newValue, oldValue) {
        //    debugger;
        //    $scope.isAuth = $window.sessionStorage["isAuth"];
        //    $scope.nomPrenom = $window.sessionStorage["nomPrenom"];
        //    console.log($scope.isAuth);

        //});

        //Save in cookie the email only
        document.cookie = "email=" + $scope.userName;
   
        $scope.logout = function () {
            $window.sessionStorage["userInfo"] = "";
            $window.sessionStorage["nomPrenom"] = "";
            $window.sessionStorage["userName"] = "";
            $window.sessionStorage["isAuth"] = "";
            $window.sessionStorage["password"] = "";

            localStorage["praticicensResult"] = "";
            localStorage["praticienRdv"] = "";
            $scope.isAuth = false;
            $location.path('/home');
            location.reload();
            //libérer les cookies
            document.cookie = "username" + '=; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
        };
      
    });