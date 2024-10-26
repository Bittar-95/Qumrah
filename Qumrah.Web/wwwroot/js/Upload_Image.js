function disableErrorCase() {
    const errorCase = document.getElementById('ErrorCase');
    errorCase.style.display = 'none'; // Hides the alert
}

function changeImage() {
    // Trigger click on the hidden file input
    document.getElementById('File').click();
}

function previewImage(event) {
    const file = event.target.files[0];
    const reader = new FileReader();

    reader.onload = function (e) {
        // Change the src of the image to the newly selected file
        const preview = document.getElementById('preview');
        preview.src = e.target.result;
        document.getElementById('removeImage').style.display = 'block'; // Show the remove button
    };

    if (file) {
        reader.readAsDataURL(file);
    }
}

function allowDrop(event) {
    event.preventDefault(); // Prevent default behavior (Prevent file from being opened)
    const leftside = document.getElementById('userImage');
    leftside.classList.add('dragover');
}

function dropImage(event) {
    event.preventDefault(); // Prevent default behavior

    const leftside = document.getElementById('userImage');
    leftside.classList.remove('dragover');

    const file = event.dataTransfer.files[0]; // Get the dropped file
    if (file && file.type.startsWith('image/')) {
        const reader = new FileReader();
        reader.onload = function (e) {
            const preview = document.getElementById('preview');
            preview.src = e.target.result; // Set the image source
            document.getElementById('removeImage').style.display = 'block'; // Show the remove button
        };
        reader.readAsDataURL(file); // Read the image file as a data URL
        
        // Update the file input with the dropped file
        const input = document.getElementById('File');
        const dataTransfer = new DataTransfer(); // Create a new DataTransfer object
        dataTransfer.items.add(file); // Add the dropped file to it
        input.files = dataTransfer.files; // Set the input's files property
    } else {
        alert('Please drop an image file.');
    }
}

function removeImage() {
    // Reset the image preview
    const preview = document.getElementById('preview');
    preview.src = './images/upload_icon_blue.png'; // Reset to default icon

    // Hide the remove button
    document.getElementById('removeImage').style.display = 'none';

    // Clear the file input
    const input = document.getElementById('File');
    input.value = ''; // Clear the input
}



// Remove the dragover class when dragging leaves
document.getElementById('userImage').addEventListener('dragleave', function() {
    this.classList.remove('dragover');
});
















// Set Arabic datetime localization for datepicker
//$.datepicker.setDefaults({
//    closeText: 'إغلاق',
//    prevText: 'السابق',
//    nextText: 'التالي',
//    currentText: 'اليوم',
//    monthNames: ['يناير', 'فبراير', 'مارس', 'أبريل', 'مايو', 'يونيو', 'يوليو', 'أغسطس', 'سبتمبر', 'أكتوبر', 'نوفمبر', 'ديسمبر'],
//    monthNamesShort: ['ينا', 'فبرا', 'مارس', 'أبريل', 'مايو', 'يونيو', 'يوليو', 'أغسطس', 'سبت', 'أكتوبر', 'نوفمبر', 'ديسمبر'],
//    dayNames: ['الأحد', 'الاثنين', 'الثلاثاء', 'الأربعاء', 'الخميس', 'الجمعة', 'السبت'],
//    dayNamesShort: ['أحد', 'اثنين', 'ثلاثاء', 'أربعاء', 'خميس', 'جمعة', 'سبت'],
//    dayNamesMin: ['أح', 'اث', 'ثلا', 'أرب', 'خم', 'جم', 'سبت'],
//    weekHeader: 'أسبوع',
//    dateFormat: 'dd/mm/yy', // Adjust the date format as needed
//    firstDay: 0,
//    isRTL: true
//});

//$(function() {
//    $("#arabicDatePicker").datepicker();
//});