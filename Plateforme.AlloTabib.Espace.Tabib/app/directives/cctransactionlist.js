'use strict';

/**
 * @ngdoc directive
 * @name pannelloFrodiKolstApp.directive:ccTransactionList
 * @description
 * # ccTransactionList
 */
angular.module('pannelloFrodiKolstApp')
  .directive('ccTransactionList', function () {
      return {
          templateUrl: '/app/views/templates/searchcreditcardtransactions.html',
          restrict: 'AEC',
          link: function postLink(scope, element, attrs) {
          }
      };
  });