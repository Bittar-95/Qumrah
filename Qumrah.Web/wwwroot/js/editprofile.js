function changeImage() {
    // Trigger click on the hidden file input
    document.getElementById('ImageProfile').click();
}

function previewImage(event) {
    const file = event.target.files[0];
    const reader = new FileReader();

    reader.onload = function(e) {
        // Change the src of the image to the newly selected file
        const profileImage = document.getElementById('profileImage');
        profileImage.src = e.target.result;
    };

    if (file) {
        reader.readAsDataURL(file);
    }
}