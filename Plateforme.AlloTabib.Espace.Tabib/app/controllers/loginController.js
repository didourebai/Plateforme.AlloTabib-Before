'use strict';

app.controller('loginController', function ($scope, $location, appSettings, $rootScope, $http, authService, $window, praticienApiServices) {
    $.unblockUI();
    var _authentication = {
        isAuth: false,
        userName: "",
        nomPrenom:""
    };

  

    $('#invalidMsg').hide();
  
 
    $scope.logout = function () {
 
        $window.sessionStorage["userInfo"] = "";
        $window.sessionStorage["nomPrenom"] = "";
        $window.sessionStorage["userName"] = "";
        $window.sessionStorage["isAuth"] = "";
        $window.sessionStorage["password"] = "";

        $rootScope.isAuth = false;

        $rootScope.isAuth = $window.sessionStorage["isAuth"];
        $rootScope.userName = $window.sessionStorage["userName"];
        $rootScope.nomPrenom = $window.sessionStorage["nomPrenom"];
        
        //libérer les cookies
        document.cookie = "username" + '=; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    };

    $scope.init = function() {
        $scope.userName = "";
        $scope.password = "";
    };
    $scope.connectedAndRedirectedToHome = function () {
        $scope.hideLoginValidationError = true;
        $.blockUI();
        var userData = {
            userName: $scope.userName,
            password: $scope.password
        };

        praticienApiServices.getPraticienByEmail(userData.userName).then(
                     // Case : Success.
                     function (results) {
                         $.unblockUI();
                         $scope.hideLoginValidationError = false;
                         _authentication.nomPrenom = results.data.data.nomPrenom;

                     },
                     // Case : Error.
                     function (error) {
                         $.unblockUI();
                         $scope.errorMessageLoginTxt = "Veuillez vérifier vos identifiants.";
                     }
                 );
        $.blockUI();
        authService.isUserAuthenticated(userData).then(
           function (result) {
               $.unblockUI();
               if (result.data == "false") {
                   $scope.errorLoginIn = 'Veuillez vérifier vos identifiants : email /mot de passe.';
                   $("#invalidMsg").slideDown("slow");
               } else {
                   
                  
                  
                   var d = new Date();

                   //var expires = "expires=" + d.toGMTString();
                   //document.cookie = "username=" + $scope.userName + "; " + expires;

                   //document.cookie = "password=" + $scope.password + "; " + expires;

                 
                   $http.defaults.headers.common.Authorization = 'Basic ' + window.btoa($scope.userName + ':' + $scope.password);

                   $('#invalidMsg').hide();
                   $scope.errorLoginIn = '';

                   //Get the profile of the account connected
                   _authentication.isAuth = true;

                 
                   _authentication.userName = $scope.userName;

                  

                   $window.sessionStorage["userInfo"] = _authentication;
                   $window.sessionStorage["nomPrenom"] = _authentication.nomPrenom;
                   $window.sessionStorage["userName"] = _authentication.userName;
                   $window.sessionStorage["isAuth"] = _authentication.isAuth;
                   $window.sessionStorage["password"] = $scope.password;

                   $rootScope.isAuth = $window.sessionStorage["isAuth"];
                   
                   $rootScope.loggedInUser = _authentication.userName;
                   
                  
                   $scope.$watch('isAuth', function () {

                       $rootScope.isAuth = $window.sessionStorage["isAuth"];
                       $rootScope.userName = $window.sessionStorage["userName"];
                       $rootScope.nomPrenom = $window.sessionStorage["nomPrenom"];

                   }, true);

                   //$window.location.href = "#/calendrier";
                   $location.path('/calendrier');
                   
               }

           },
           function (error) {
               $.unblockUI();
               $scope.errorLoginIn = "Error avec notre serveur, veuillez consulter votre compte plus tard. Merci pour votre compréhension.";
               $("#invalidMsg").slideDown("slow");
           }

       );
        $.unblockUI();
    };

 $(function () {
        $("#closeAlert").on("click", function () {
            $('#invalidMsg').hide();
        });
    });

});