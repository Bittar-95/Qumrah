function closeButton1(event) {
    // Access the element directly using its ID
    var errorCase = document.getElementById('ErrorCase1');
    if (errorCase) {
        errorCase.style.display = 'none';
    }
}

// Add event listener directly to the element with the ID 'ErrorCase1'
document.getElementById('ErrorCase1').addEventListener('click', closeButton1);


////////////////////////////////////////////////////////////////////////






