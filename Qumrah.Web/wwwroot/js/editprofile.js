function changeImage() {
    // Trigger click on the hidden file input
    document.getElementById('ImageProfile').click();
}

function previewImage(event) {
    const file = event.target.files[0];
    const reader = new FileReader();

    reader.onload = function (e) {
        // Change the src of the image to the newly selected file
        const profileImage = document.getElementById('profileImage');
        profileImage.src = e.target.result;
    };

    if (file) {
        reader.readAsDataURL(file);
    }
}
// Get references to the button and the update profile box
const updateProfileBox = document.getElementById('update-profile-box');

function showUpdateProfileBox() {
    // Add a click event listener to the button
    // Show the update profile box
    updateProfileBox.style.display = 'block';

    // Hide the update profile box after 3 seconds
    setTimeout(function () {
        updateProfileBox.style.display = 'none';
    }, 3000);
}
