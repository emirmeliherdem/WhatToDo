$(document).ready(function () {
    $('.ActiveCheck2').change(function () {
        var self = $(this);
        var id = self.attr('id');
        var value = self.prop('checked');

        $.ajax({
            url: '/Habits/AJAXEdit',
            data: {
                id: id,
                value: value
            },
            type: 'POST',
            success: function (result) {
                $('#habitTableDiv').html(result);
            }
        });
    });
})