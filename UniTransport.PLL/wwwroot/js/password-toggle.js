document.addEventListener('DOMContentLoaded', function() {
    // Get all password toggle buttons
    const toggleButtons = document.querySelectorAll('.toggle-password');

    toggleButtons.forEach(button => {
        button.addEventListener('click', function(e) {
            e.preventDefault();
            
            // Get the target password input
            const targetId = this.dataset.target || 'password';
            const passwordInput = document.getElementById(targetId);
            const eyeIcon = this.querySelector('i');

            if (passwordInput && eyeIcon) {
                // Toggle the password visibility
                if (passwordInput.type === 'password') {
                    passwordInput.type = 'text';
                    eyeIcon.classList.remove('fa-eye');
                    eyeIcon.classList.add('fa-eye-slash');
                } else {
                    passwordInput.type = 'password';
                    eyeIcon.classList.remove('fa-eye-slash');
                    eyeIcon.classList.add('fa-eye');
                }
            }
        });
    });
});
