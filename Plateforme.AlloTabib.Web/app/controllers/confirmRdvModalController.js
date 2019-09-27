'use strict';

app.controller('confirmRdvModalController', function ($scope, $location, $modalInstance, $window, praticienCin, dateRendezVous, contactApiServices, horaireRendezVous, specialiteRdv, nomPrenomMe, adressePraticien, telephonePraticien, rendezvousApiServices) {
    

    

    var praticien = praticienCin;
    var dateSelectionne = dateRendezVous;
    var heurerdv = horaireRendezVous;
    
    var userName = $window.sessionStorage["userName"];
    if (userName == '' || userName == null || userName == undefined)
        $location.path('/loginpatient');


    $scope.dateRendezVous = dateRendezVous;
    $scope.horaireRendezVous = horaireRendezVous;
    $scope.specialiteRdv = specialiteRdv;
    $scope.nomPrenomMe = nomPrenomMe;
    

    function sendEmail(contactObject) {
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

    $scope.confirmRdv = function () {
        $.blockUI();
        //userName  :email patient
        $scope.confirmRdv = {
            statut: "NC",
            patientCin: userName,
            praticienCin: praticien,
            heureDebut: heurerdv,
            currentDate: dateSelectionne
        };
        rendezvousApiServices.addRendezVous($scope.confirmRdv).then(
                        // Case : Success.
                        function (result) {
                            $.unblockUI();

                            //Envoyer un Email au patient pour dire qu'il  a passé un rendez vous
                            $scope.contactObject = {
                                from: "contact@allotabib.net",
                                to: userName,
                                sujet: "[AlloTabib] Prise de rendez vous à confirmer",
                                body: "Bonjour,\nVotre demande de rendez-vous a été envoyée au calendrier du médecin pour confirmation.\nVous allez recevoir un email de confirmation dès que votre rendez-vous sera confirmé.\nInformations rendez-vous : \n\nDate du rendez-vous : " + "le " + dateRendezVous + "à " + horaireRendezVous + "\n" + "Informations sur le spécialiste : \n" + nomPrenomMe + "\n spécialité : " + specialiteRdv + "\nAdresse : " + adressePraticien + "\nTéléphone : " + telephonePraticien + ".\nSi vous avez des questions veuillez nous contacter par e-mail: contact@allotabib.net!" + "\n\n\n---Cordialement\nEquipe AlloTabib."
                           };
                           
                            sendEmail($scope.contactObject);

                            //Mail leya ena
                            $scope.contactObject1 = {
                                from: "contact@allotabib.net",
                                to: "didourebai@gmail.com",
                                sujet: "[AlloTabib] Prise de rendez vous à confirmer",
                                body: "Bonjour,\nVotre demande de rendez-vous a été envoyée au calendrier du médecin pour confirmation.\nVous allez recevoir un email de confirmation dès que votre rendez-vous sera confirmé.\nInformations rendez-vous : \n\nDate du rendez-vous : " + "le " + dateRendezVous + "à " + horaireRendezVous + "\n" + "Informations sur le spécialiste : \n" + nomPrenomMe + "\n spécialité : " + specialiteRdv + "\nAdresse : " + adressePraticien + "\nTéléphone : " + telephonePraticien + ".\nSi vous avez des questions veuillez nous contacter par e-mail: contact@allotabib.net!" + "\n\n\n---Cordialement\nEquipe AlloTabib."
                            };

                            sendEmail($scope.contactObject1);

                            toastr.success("Rendez vous pris le" + dateRendezVous + "à l'heure :" + heurerdv, "Rendez vous à confirmer!");
                            $modalInstance.dismiss("cancel");
                            $location.path('/rendezvous');
                        },

                        // Case : Error.
                        function (error) {
                            $.unblockUI();
                            toastr.error("Votre rendez vous n'a pas été pris ! veuillez choisir une autre date.");
                            $modalInstance.dismiss("cancel");
                            $location.path('/rendezvous');
                        }
                    );
    };
    
    $scope.cancelAndLeave = function () {
        $modalInstance.dismiss("cancel");
       
    };
});