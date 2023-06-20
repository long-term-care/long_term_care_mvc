$(document).ready(function () {
    closeMessageBox
    var volunteerMsgBox = $('#volunteerMsgBox');
    var elderMsgBox = $('#elderMsgBox');
    var translucentOverlay = '<div class="translucent-overlay"></div>';
    var messageBox = $('#messageBox');
    var message_Box = $('.message-box');
    var generatedOrderNumbers = [];

    function generateOrderNumber() {
        var orderNumber;
        var isDuplicate = true;

        while (isDuplicate) {
            orderNumber = "A" + (Math.floor(Math.random() * 999999) + 1).toString().padStart(6, '0');
            isDuplicate = generatedOrderNumbers.includes(orderNumber);
        }

        generatedOrderNumbers.push(orderNumber);
        return orderNumber;
    }


    $("#CaseView4Other").change(function () {
        if ($(this).is(":checked")) {
            $("input[name='CaseView4']").prop("disabled", false);
        } else {
            $("input[name='CaseView4']").prop("disabled", true);
        }
    });
    $("#caseView1No").change(function () {
        if ($(this).is(":checked")) {
            $("input[name='CaseView1']").prop("disabled", false);
        } else {
            $("input[name='CaseView1']").prop("disabled", true);
        }
    });

    $("#CaseView6AO").change(function () {
        if ($(this).is(":checked")) {
            $("input[name='CaseView6']").prop("disabled", false);
        } else {
            $("input[name='CaseView6']").prop("disabled", true);
        }
    });

    $('#orderNumberInput').val(generateOrderNumber());
    $('#orderNumberInputCar').val(generateOrderNumber());


    $('.question input[type="checkbox"]').change(function () {
        $(this).siblings('input[type="checkbox"]').prop('checked', false);
    });

    $('#volunteer, #elder').on('click', function () {
        var id = $(this).attr('id');
        var otherId = (id === 'volunteer') ? 'elder' : 'volunteer';
        $('#register, #identity').addClass('hidden');
        $('#fullscreen-image').css('background-image', 'url("../Resources/background.png")');
        volunteerMsgBox.toggleClass('hidden', id !== 'volunteer').toggleClass('large', id === 'volunteer');
        elderMsgBox.toggleClass('hidden', id !== 'elder').toggleClass('large', id === 'elder');
        $(this).add('#' + otherId).hide();
        $('body').append(translucentOverlay);
    });
    $('#close, #closeVolunteer, #closeElder').on('click', function () {
        closeMessageBox();
    });
    $('.closeMessageBox').not('#closeVolunteer, #closeElder, #close').on('click', function () {
        closeMessageBoxBTN();
    });
    function closeMessageBox() {
        volunteerMsgBox.addClass('hidden');
        elderMsgBox.addClass('hidden');
        message_Box.fadeOut();
        messageBox.fadeOut();
        $('.translucent-overlay').remove();
    }
    $('.closeMessageBox').click(function (e) {
        e.preventDefault();
        var valid = true;
        $('input[required]').each(function () {
            if ($(this).val() === '') {
                valid = false;
                return false;
            }
        });
        if ($('option').val() === 'na') {
            valid = false;
        }
        if (valid) {
            $(this).closest('.container').parent().addClass('hidden');
        }
    });


    var currentYear = new Date().getFullYear();
    var startYear = currentYear - 112;
    var endYear = currentYear + 112;
    var yearSelect = $('.yearSelect');
    for (var year = startYear; year <= endYear; year++) {
        yearSelect.append($('<option>', {
            value: year,
            text: year
        }));
    }
    var months = ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'];
    var monthSelect = $('.monthSelect');
    $.each(months, function (index, month) {
        var monthValue = index + 1;
        monthSelect.append($('<option>', {
            value: monthValue,
            text: month
        }));
    });
    var daysInMonth = 31;
    var daySelect = $('.daySelect');
    for (var day = 1; day <= daysInMonth; day++) {
        daySelect.append($('<option>', {
            value: day,
            text: day
        }));
    }

    $('#renewPassword').click(function () {
        $('body').append(translucentOverlay);
        messageBox.fadeIn().addClass('large');
    });
});
