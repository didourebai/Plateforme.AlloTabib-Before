'use strict';
app.controller('navigationController',
    function ($scope, $location, $rootScope,appSettings, $http, authService, $window) {

       

        $scope.isActive = function (path) {
            return $location.path().substr(0, path.length) == path;
        };

       
        init();
        function init() {
            $scope.authentication = $window.sessionStorage["userInfo"];
            $scope.userName = $window.sessionStorage["userName"];
            $scope.isAuth = $window.sessionStorage["isAuth"];
            $scope.nomPrenom = $window.sessionStorage["nomPrenom"];
            
            $rootScope.loggedInUser = $window.sessionStorage["userName"];
        }

        //Save in cookie the email only
        document.cookie = "email=" + $scope.userName;
      
    });