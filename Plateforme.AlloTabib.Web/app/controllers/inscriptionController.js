'use strict';

app.controller('inscriptionController', function ($scope, $location, $window, authService, patientApiServices, contactApiServices, $http, $rootScope) {

    $.unblockUI();
    var _authentication = {
        isAuth: false,
        userName: "",
        nomPrenom: ""
    };

    $scope.sucessMessageTxt = undefined;
    $scope.errorMessageTxt = undefined;
    $scope.loginHasErrors = false;
    $scope.addHasErrors = false;
    $scope.clicked = false;
    $scope.errorMessageLoginTxt = undefined;

    $scope.errorSentEmail = false;
    $scope.isEmailSent = false;

  $scope.loadInputClassForAdd = function (valid) {

      if ($scope.clicked == false) {
          return 'is-not-clicked';
      }
      if (valid) {
          return 'has-success';
      } else {
          return 'has-error';
      }
  };
     $scope.specialites = [
        'ANESTHESIE-REANIMATION',
        'CARCINOLOGIE MEDICALE',
        'CARDIOLOGIE',
        'CHI. CARDIOVASCULAIRE T',
        'CHIRURGIE CARCINOLOGIE',
        'CHIRURGIE GENERALE',
        'CHIRURGIE INFANTILE',
        'CHIRURGIE MAXILLO-FACIALE',
        'CHIRURGIE ORTHOPEDIQUE',
        'CHIRURGIE PLASTIQUE',
        'DERMATOLOGIE',
        'ENDOCRINOLOGIE',
        'GASTRO-HEPATHOLOGIE',
        'GYNECOLOGIE-OBSTETRIQUE',
        'HEMATOLOGIE',
        'MALADIES INFECTIEUSES',
        'MEDECINE GENERALE',
        'MEDECINE INTERNE',
        'MEDECINE LEGALE',
        'MEDECINE NUCLEAIRE',
        'MEDECINE PHYSIQUE',
        'NEUROCHIRURGIE',
        'NEUROLOGIE',
        'NUTRITION-DIETETIQUE',
        'OPHTALMOLOGIE',
        'ORL ET STOMATOLOGIE',
        'PARASITOLOGIE',
        'PEDIATRIE',
        'PNEUMOLOGIE',
        'PSYCHIATRIE',
        'RADIOLOGIE',
        'RHUMATOLOGIE',
        'UROLOGIE'
    ];
    $scope.Isclicked = function() {
        $scope.clicked = true;
    };
    

    $scope.internalerror = false;

    $scope.envoyerMotpasseOublie = function () {
        $scope.errorSentEmail = false;
        $scope.isEmailSent = false;
        $scope.internalerror = false;
       
        if( $scope.emailmotdepasse == undefined ||  $scope.emailmotdepasse == '')
        {
            
            toastr.error("Veuillez saisir votre email.");
        }
        else {
           
            authService.sendEmailMotDePasseO($scope.emailmotdepasse)
            .then(
            function (results) {
               
                $.unblockUI();
                $scope.errorSentEmail = false;
                $scope.isEmailSent = true;
                $scope.internalerror = false;
                toastr.success("Un email a été envoyé, veuillez vérifier votre boite.");

            },
              // Case : Error.
               function (error) {
                   $.unblockUI();
                   $scope.errorSentEmail = true;
                   $scope.isEmailSent = false;
                   $scope.internalerror = true;
                   toastr.error("Veuillez vérifier votre email ou nous contacter en cas de problème.");
                   
               }
            );
            

        }
        //Email 

    };

    function sendEmail(contactObject) {
        $.blockUI();
        contactApiServices.sendEmail(contactObject)
           .then(
               // Case : Success.
               function (results) {
                   $.unblockUI();
                   $scope.newContact = {};

                   $scope.isSuccessfullyAdded = true;
                   $scope.addHasErrors = false;
               },
               // Case : Error.
               function (error) {
                   $.unblockUI();
                   //Afficher panel des erreurs
                   $scope.addHasErrors = true;
                   //ne pas afficher le message de succès
                   $scope.isSuccessfullyAdded = false;

                   // Add the errors messages.

                   if (error.status == 400) {
                       //if (error.data.errors[0].type == 'VALIDATION_FAILURE') {
                       //    $scope.addErrorType = "Erreur de validation";
                       //    $scope.errorMessageTxt = error.data.errors[0].message;
                       //}
                   }

                   if (error.status == 401) {
                       //$scope.addErrorMessage = JSON.parse(error.data);
                       //$scope.errorMessageTxt = "Non autorisé";
                   }

                   if (error.status == 500) {
                       //$scope.addErrorType = "Erreur lié au serveur";
                       //$scope.errorMessageTxt = error.data.errors[0].exception.Message;
                   }
                   //$scope.addErrorType = "Erreur anonyme";
                   //$scope.errorMessageTxt = error.data.toString();
               }
           );
    }

    /** Partie d'ajout d'un patient **/
    $scope.addPatient = function () {
        $.blockUI();
        $scope.hideValidationError = true;
        $scope.addHasErrors = false;
        // Disable the edit button click and convert it to 'Loading'.
        $('#addPatientBtn').button('loading');

        // Disable the cancel button click.
        //document.getElementById("addPatientBtn").disabled = true;
        document.getElementById("canceladdPatientBtn").disabled = true;


      patientApiServices.addPatient($scope.newPatient)
             .then
             (
                 // Case : Success.
                 function (results) {
                     $.unblockUI();
                     window.scrollTo(0, 0);
                     //Envoyer un email au patient
                     $scope.contactObject = {
                         from: "contact@allotabib.net",
                         to: $scope.newPatient.email,
                         sujet: "[AlloTabib] Confirmation d'inscription chez AlloTabib",
                         body: "Bonjour," + $scope.newPatient.nomPrenom + ",  \n nous vous remercieront pour l’intérêt porté à notre Plateforme AlloTabib., Vous pouvez accéder à votre espace et commencer à prendre des rendez vous, veuillez respecter vos rendez vous pris.\n Cordialement \n Equipe AlloTabib."
                     };

                     sendEmail($scope.contactObject);

                     //Mail pour moi
                     $scope.contactObject1 = {
                         from: "contact@allotabib.net",
                         to: "didourebai@gmail.com",
                         sujet: "[AlloTabib] Confirmation d'inscription chez AlloTabib",
                         body: "Bonjour," + $scope.newPatient.nomPrenom + ",  \n Nous vous remercieront pour l’intérêt porté à notre Plateforme AlloTabib., Vous pouvez accéder à votre espace et commencer à prendre des rendez vous, veuillez respecter vos rendez vous pris.\n Cordialement \n Equipe AlloTabib."
                     };
                     sendEmail($scope.contactObject1);

                     $scope.newPatient.cin = results.data.data.cin;
                     $scope.newPatient = {};
                     //$scope.sucessMessageTxt = "Votre enregistrement a été effectué avec succès, vous allez reçevoir au bout de 24h un email de confirmation d'inscription.";
                     toastr.success("Votre enregistrement a été effectué avec succès, vous allez reçevoir au bout de 24h un email de confirmation d'inscription.");
                     $scope.addHasErrors = false;
                     $scope.isSuccessfullyAdded = true;
                     $scope.hideValidationError = false;

                     // Reset the add and delete buttons.
                     $('#addPatientBtn').button('reset');
                     document.getElementById("canceladdPatientBtn").disabled = false;
                 },
                 // Case : Error.
                 function (error) {
                     $.unblockUI();
                     $scope.addHasErrors = true;
                     $scope.isSuccessfullyAdded = false;
                     // Add the errors messages.
                     $scope.addErrorType = error.data.errors[0].type;

                     if (error.status == 500) {
                         $scope.errorMessageTxt = error.data.errors[0].exception.Message;
                     } else {
                         $scope.errorMessageTxt = error.data.errors[0].message;
                     }
                     // Reset the add and delete buttons.
                     $('#addPatientBtn').button('reset');
                     document.getElementById("canceladdPatientBtn").disabled = false;

                 }
             );
    };

    $scope.loginPatient = function () {
        $scope.hideLoginValidationError = true;
        $.blockUI();
        $scope.loginHasErrors = false;
        var userData = {
            userName: $scope.loginPatient.email,
            password: $scope.loginPatient.password,
            type:"patient"
    };

        patientApiServices.getPatientByEmail(userData.userName).then(
                     // Case : Success.
                     function (results) {
                         $scope.loginHasErrors = false;
                         _authentication.nomPrenom = results.data.data.nomPrenom;
                         $scope.hideLoginValidationError = false;
                     },
                     // Case : Error.
                     function (error) {
                         console.log(error);
                         $scope.loginHasErrors = true;
                         $scope.errorMessageLoginTxt = "Veuillez vérifier vos identifiants ou votre compte n'est pas active. Veuillez nous contacter.";
                     }
                 );

        authService.isUserAuthenticated(userData).then(
      function (result) {
       $.unblockUI();
       if (result.data == "false") {
           $scope.loginHasErrors = true;
           $scope.errorMessageLoginTxt = 'Veuillez vérifier vos identifiants : email /mot de passe.';
           $("#invalidMsg").slideDown("slow");
       } else {
           $scope.loginHasErrors = false;
       

           var d = new Date();

           var expires = "expires=" + d.toGMTString();
           document.cookie = "username=" + $scope.userName + "; " + expires;

           document.cookie = "password=" + $scope.password + "; " + expires;


           $http.defaults.headers.common.Authorization = 'Basic ' + window.btoa($scope.userName + ':' + $scope.password);

           $('#invalidMsg').hide();
           $scope.errorLoginIn = '';

           //Get the profile of the account connected
           _authentication.isAuth = true;


           _authentication.userName = $scope.loginPatient.email;



           $window.sessionStorage["userInfo"] = _authentication;
           $window.sessionStorage["nomPrenom"] = _authentication.nomPrenom;
           $window.sessionStorage["userName"] = _authentication.userName;
           $window.sessionStorage["isAuth"] = _authentication.isAuth;
           $window.sessionStorage["password"] = $scope.password;

           
           $scope.$watch('isAuth', function () {
               
               $rootScope.isAuth = $window.sessionStorage["isAuth"];
               $rootScope.userName = $window.sessionStorage["userName"];
               $rootScope.nomPrenom = $window.sessionStorage["nomPrenom"];

           }, true);


           if (localStorage["praticienRdv"] != null && localStorage["praticienRdv"] != "undefined" && localStorage["praticienRdv"] != "") {
               $location.path('/rendezvous');
           } else {
               $location.path('/service');
           }

           //location.reload();

       }

   },
   function (error) {
       $.unblockUI();
       $scope.loginHasErrors = true;
       $scope.errorMessageLoginTxt = "Error avec notre serveur, veuillez consulter votre compte plus tard. Merci pour votre compréhension.";
       $("#invalidMsg").slideDown("slow");
   }
);
    };

  var errorMsg = function (msg) {
      $scope.errorMessageTxt = msg;
      $("#errorMessage").slideDown(500);
  };
  $scope.redirectToHome = function () {
      $location.path('/home');
  };
  $scope.closePanels = function () {
      $('#sucessMessage').slideUp(500);
      $location.path('/home');
  };
});