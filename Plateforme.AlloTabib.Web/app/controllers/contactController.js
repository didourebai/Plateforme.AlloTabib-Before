'use strict';

app.controller('contactController', function ($scope, $location, contactApiServices) {
   
    $scope.isSuccessfullyAdded = false;
    $scope.addHasErrors = false;
    
    $scope.sendMail = function () {
        $.blockUI();
        $scope.hideValidationError = true;
        var body = "Nom : "+" " + $scope.newContact.nom + " proposition :" + $scope.newContact.message;
        
        if ($scope.newContact.nom != '' && $scope.newContact.nom != null && $scope.newContact.message != '' && $scope.newContact.email != '' && $scope.newContact.sujet != '') {
            contactApiServices.contacts($scope.newContact.email, $scope.newContact.sujet, body)
    .then(
        // Case : Success.
        function (results) {
            $.unblockUI();
            $scope.newContact = {};
            //$location.path('/home');
            toastr.success("Votre propositon a été envoyé avec succès.");

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
                if (error.data.errors[0].type == 'Validation des champs') {
                    $scope.addErrorType = "Erreur de validation";
                    $scope.errorMessageTxt = error.data.errors[0].message;
                }
            }

            if (error.status == 401) {
                $scope.addErrorMessage = JSON.parse(error.data);
                $scope.errorMessageTxt = "Non autorisé";
            }

            if (error.status == 500) {
                $scope.addErrorType = "Erreur lié au serveur";
                $scope.errorMessageTxt = error.data.errors[0].exception.Message;
            }
            $scope.addErrorType = "Erreur anonyme";
            $scope.errorMessageTxt = error.data.toString();
        }
    );
        }

    };
    $scope.addContactNew = {};
    $scope.postEmail = function () {
        $.blockUI();
        $scope.hideValidationError = true;
        var body = "Nom : " + $scope.newContact.nom + "proposition :" + $scope.newContact.message;

        $scope.addContactNew = {
            from: $scope.newContact.email,
            to: 'plateforme.allotabib@gmail.com',
            sujet: $scope.newContact.sujet,
            body: body
        };
        
        contactApiServices.addMail($scope.addContactNew)
            .then(
                // Case : Success.
                function (results) {
                    $.unblockUI();
                    $scope.newContact = {};
                    //$location.path('/home');
                  
                    toastr.success("Votre propositon a été envoyé avec succès.");
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
                        if (error.data.errors[0].type == 'VALIDATION_FAILURE') {
                            $scope.addErrorType = "Erreur de validation";
                            $scope.errorMessageTxt = error.data.errors[0].message;
                        }
                    }

                    if (error.status == 401) {
                        $scope.addErrorMessage = JSON.parse(error.data);
                        $scope.errorMessageTxt = "Non autorisé";
                    }

                    if (error.status == 500) {
                        $scope.addErrorType = "Erreur lié au serveur";
                        $scope.errorMessageTxt = error.data.errors[0].exception.Message;
                    }
                    $scope.addErrorType = "Erreur anonyme";
                    $scope.errorMessageTxt = error.data.toString();
                }
            );
    };

    $scope.closePanels = function () {
        $('#sucessMessage').slideUp(500);
        $('#errorMessage').slideUp(500);
    };
});