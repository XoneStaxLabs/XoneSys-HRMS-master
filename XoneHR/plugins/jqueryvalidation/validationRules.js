$('.required').each(function () {   
    $(this).rules('add', {
        required: true        
    });
});

$('.email').each(function () {
    $(this).rules('add', {
        email: true
    });
});


