// Password validation functionality
function initPasswordValidation(passwordInput, confirmInput, messageElement) {
    const strengthIndicator = document.querySelector('.password-strength');

    function updatePasswordStrength(password) {
        let score = 0;

        // Length check
        if (password.length >= 8) score++;
        if (password.length >= 12) score++;

        // Character type checks
        if (/[A-Z]/.test(password)) score++;
        if (/[a-z]/.test(password)) score++;
        if (/[0-9]/.test(password)) score++;
        if (/[^A-Za-z0-9]/.test(password)) score++;

        // Update strength class
        strengthIndicator.classList.remove('weak', 'medium', 'strong');
        if (score >= 5) {
            strengthIndicator.classList.add('strong');
        } else if (score >= 3) {
            strengthIndicator.classList.add('medium');
        } else if (score > 0) {
            strengthIndicator.classList.add('weak');
        }
    }

    function checkPasswordRequirements(password) {
        const requirements = {
            length: password.length >= 8,
            uppercase: /[A-Z]/.test(password),
            lowercase: /[a-z]/.test(password),
            number: /[0-9]/.test(password),
            special: /[^A-Za-z0-9]/.test(password)
        };

        // Update requirement list items
        Object.keys(requirements).forEach(req => {
            const li = document.getElementById('signup-' + req); // Updated to match HTML IDs
            if (li) {
                if (requirements[req]) {
                    li.classList.add('valid');
                } else {
                    li.classList.remove('valid');
                }
            }
        });

        return Object.values(requirements).every(req => req);
    }

    // Password input validation
    if (passwordInput) {
        passwordInput.addEventListener('input', function () {
            const password = this.value;
            const isValid = checkPasswordRequirements(password);
            updatePasswordStrength(password);

            if (!isValid && messageElement) {
                messageElement.textContent = 'Please meet all password requirements';
                messageElement.style.display = 'block';
            } else if (messageElement) {
                messageElement.style.display = 'none';
            }
        });

        // Show requirements on focus
        passwordInput.addEventListener('focus', function () {
            const requirements = this.closest('.password-container').querySelector('.password-requirements');
            if (requirements) {
                requirements.style.display = 'block';
            }
        });

        // Hide requirements on blur if not hovering
        passwordInput.addEventListener('blur', function () {
            const container = this.closest('.password-container');
            const requirements = container.querySelector('.password-requirements');

            if (requirements && !requirements.matches(':hover')) {
                requirements.style.display = 'none';
            }
        });
    }

    // Keep requirements visible while hovering
    document.querySelectorAll('.password-requirements').forEach(req => {
        req.addEventListener('mouseenter', function () {
            this.style.display = 'block';
        });

        req.addEventListener('mouseleave', function () {
            if (!this.closest('.password-container').querySelector('input:focus')) {
                this.style.display = 'none';
            }
        });
    });

    return {
        checkPasswordRequirements,
        updatePasswordStrength
    };
}

// Export the initialization function
window.initPasswordValidation = initPasswordValidation;

document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('signup-form'); // Updated ID to match the HTML
    const pass0 = document.getElementById('signup-password'); // Updated ID to match the HTML
    const pass1 = document.getElementById('signup-confirm-password'); // Updated ID to match the HTML
    const message = document.getElementById('signup-message'); // Updated ID to match the HTML
    const emailInput = document.getElementById('signup-email'); // Updated ID to match the HTML
    const phoneInput = document.getElementById('signup-phone'); // Updated ID to match the HTML
    const studentIdInput = document.getElementById('signup-student-id'); // Updated ID to match the HTML

    // Password validation requirements
    const requirements = {
        length: /.{8,}/,
        uppercase: /[A-Z]/,
        lowercase: /[a-z]/,
        number: /[0-9]/,
        special: /[!@#$%^&*(),.?":{}|<>]/
    };

    // Update password requirements list as user types
    pass0.addEventListener('input', function () {
        const password = this.value;
        let strength = 0;

        // Check each requirement
        for (let [key, regex] of Object.entries(requirements)) {
            const li = document.getElementById('signup-' + key); // Updated ID to match the HTML
            const meets = regex.test(password);
            li.style.color = meets ? 'green' : 'red';
            if (meets) strength++;
        }

        // Update password strength indicator
        const strengthDiv = document.querySelector('.password-strength');
        const percentage = (strength / 5) * 100;
        strengthDiv.style.width = percentage + '%';
        strengthDiv.style.backgroundColor =
            percentage <= 20 ? 'red' :
                percentage <= 40 ? 'orange' :
                    percentage <= 60 ? 'yellow' :
                        percentage <= 80 ? 'lightgreen' :
                            'green';
    });

    // Check password match
    pass1.addEventListener('input', function () {
        if (this.value !== pass0.value) {
            message.textContent = 'Passwords do not match';
            message.style.display = 'block';
            message.style.color = 'red';
        } else {
            message.style.display = 'none';
        }
    });

    // Email validation
    function isValidEmail(email) {
        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
    }

    emailInput.addEventListener('input', function () {
        if (!isValidEmail(this.value)) {
            this.setCustomValidity('Please enter a valid email address');
        } else {
            this.setCustomValidity('');
        }
    });

    // Phone number validation
    phoneInput.addEventListener('input', function (e) {
        // Remove any non-digit characters
        this.value = this.value.replace(/\D/g, '');

        // Ensure it starts with 0 and has 10 digits
        if (!/^0\d{9}$/.test(this.value)) {
            this.setCustomValidity('Phone number must start with 0 and have 10 digits');
        } else {
            this.setCustomValidity('');
        }
    });

    // Student ID validation
    studentIdInput.addEventListener('input', function () {
        // Ensure it's a number and has exactly 8 digits
        if (!/^\d{8}$/.test(this.value)) {
            this.setCustomValidity('Student ID must be exactly 8 digits');
        } else {
            this.setCustomValidity('');
        }
    });

    // Form submission
    form.addEventListener('submit', function (e) {
        e.preventDefault();

        // Check if all validations pass
        const isPasswordMatch = pass0.value === pass1.value;
        const isPasswordValid = Object.values(requirements).every(regex => regex.test(pass0.value));
        const isEmailValid = isValidEmail(emailInput.value);
        const isPhoneValid = /^0\d{9}$/.test(phoneInput.value);
        const isStudentIdValid = /^\d{8}$/.test(studentIdInput.value);

        if (!isPasswordMatch) {
            message.textContent = 'Passwords do not match';
            message.style.display = 'block';
            message.style.color = 'red';
            return;
        }

        if (!isPasswordValid) {
            message.textContent = 'Password does not meet all requirements';
            message.style.display = 'block';
            message.style.color = 'red';
            return;
        }

        if (!isEmailValid || !isPhoneValid || !isStudentIdValid) {
            message.textContent = 'Please fill all fields correctly';
            message.style.display = 'block';
            message.style.color = 'red';
            return;
        }

        // If all validations pass, collect form data
        const formData = {
            username: document.getElementById('signup-username').value, // Updated ID to match the HTML
            email: emailInput.value,
            phone: phoneInput.value,
            studentId: studentIdInput.value,
            password: pass0.value
        };

        // Store user data (in real app, this would be sent to a server)
        localStorage.setItem('userData', JSON.stringify(formData));
        localStorage.setItem('isLoggedIn', 'true');

        // Show success message and redirect
        message.textContent = 'Registration successful!';
        message.style.display = 'block';
        message.style.color = 'green';

        document.getElementById('signup-form').submit();
        setTimeout(() => {
            window.location.href = 'profile.html';
        }, 1000);
    });
});