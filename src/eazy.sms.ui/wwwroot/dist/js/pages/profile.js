$(document).ready(function() {
    
    "use strict";
    
    $('.like-btn').on('click', function() {
        $(this).toggleClass('liked');
        $(this).find('i').toggleClass('fas');
        $(this).find('i').toggleClass('far');
        
        return false;
    });

});