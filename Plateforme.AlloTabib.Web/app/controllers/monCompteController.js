'use strict';

app.controller('monCompteController', function ($scope, $location, patientApiServices, $window) {
    $.unblockUI();
    var userName = $window.sessionStorage["userInfo"];

    if (userName == '' || userName == null || userName == undefined)
        $location.path('/home');
    $scope.showValidationErrorForTitle = true;
    $scope.EstEninscriptionPatient = true;
    $scope.isSuccessfullyAdded = false;
    $scope.addHasErrors = false;

    $scope.SuccessUpdate = false;

    function getCookie(cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i].trim();
            if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
        }
        return "";
    }
    var email = getCookie("email");
    Initialize();

    $scope.Isclicked = function () {
        $scope.clicked = true;
    };

    function Initialize() {
        var mail = $window.sessionStorage["userName"];
        patientApiServices.getPatientByEmail(mail)
            .then(
                // Case : Success.
                function (results) {
                    var pati = results.data.data;
                    
                    $scope.updatePatient = {
                        adresse: pati.adresse,
                        cin: pati.cin,
                        datenaissance: pati.dateNaissance,
                        email: pati.email,
                        nomPrenom: pati.nomPrenom,
                        password: pati.password,
                        sexe: pati.sexe,
                        telephone: pati.telephone,
                        password_c: pati.password
                    };
                   
                },
                // Case : Error.
                function (error) {
                    //Afficher panel des erreurs

                }
            );
    }

    //updatePraticien
    $scope.modifierPatient = function () {
       
        $.blockUI();
        $scope.hideValidationError = true;
        var patient = $scope.updatePatient;
        patientApiServices.updatePatient(patient)
            .then(
                // Case : Success.
                function (results) {
                    $.unblockUI();
                    $scope.SuccessUpdate = true;
                    //$scope.udpatePraticien = {};
                    ////$location.path('/home');
                    //$scope.sucessMessageTxt = "Votre modification a été effectué avec succès, vous allez reçevoir au bout de 24h un email de confirmation de modification.";
                    toastr.success("Votre modification a été effectué avec succès.");

                    $scope.EstEninscriptionPatient = false;
                    $scope.isSuccessfullyAdded = true;
                    $scope.addHasErrors = false;

                    $scope.udpatePraticien = prat;
                    $location.path("/home");
                },
                // Case : Error.
                function (error) {
                    $.unblockUI();
                    $scope.SuccessUpdate = false;
                    //Afficher panel des erreurs
                    $scope.addHasErrors = true;
                    //garder la page d'inscription
                    $scope.EstEninscriptionPatient = true;
                    //ne pas afficher le message de succès
                    $scope.isSuccessfullyAdded = false;

                    // Add the errors messages.

                    if (error.status == 400) {
                        if (error.data.errors[0].type == 'VALIDATION_FAILURE') {
                            $scope.addErrorType = "Erreur de validation";
                            $scope.errorMessageTxt = error.data.errors[0].message;
                            toastr.error(error.data.errors[0].message);
                        }
                    }

                    if (error.status == 401) {
                        $scope.addErrorMessage = JSON.parse(error.data);
                        $scope.errorMessageTxt = "Non autorisé";
                        toastr.error($scope.errorMessageTxt);
                    }

                    if (error.status == 500) {
                        $scope.addErrorType = "Erreur lié au serveur";
                        $scope.errorMessageTxt = error.data.errors[0].exception.Message;
                        toastr.error(error.data.errors[0].exception.Message);
                    }
                    else {
                        $scope.addErrorType = "Erreur anonyme";
                        $scope.errorMessageTxt = "Une erreur est survenue. Veuillez nous contacter.AlloTabib.";
                        toastr.error("Une erreur est survenue. Veuillez nous contacter.AlloTabib.");
                    }
                 
                }
            );
    };
});