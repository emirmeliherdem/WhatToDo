$(document).ready(function () {
    $('.ActiveDelete').change(function () {
        var self = $(this);
        var id = self.attr('id');

        $.ajax({
            url: '/ToDoes/AJAXDelete',
            data: {
                id: id,
            },
            type: 'POST',
            success: function (result) {
                $('#toDoTableDiv').html(result);
            }
        });
    });
})