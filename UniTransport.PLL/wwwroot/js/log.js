// Check for remembered credentials on page load
document.addEventListener('DOMContentLoaded', function() {
    const rememberedUser = localStorage.getItem('rememberedUser');
    if (rememberedUser) {
        const userData = JSON.parse(rememberedUser);
        document.querySelector('#login-username').value = userData.username;
        document.querySelector('#login-password').value = userData.password;
        document.querySelector('#login-remember-me').checked = true;
    }
});

// Login form handling
document.querySelector('.form').addEventListener('submit', function(event) {
    event.preventDefault();
    
    // Get form inputs
    const username = document.querySelector('#login-username').value;
    const password = document.querySelector('#login-password').value;
    const rememberMe = document.querySelector('#login-remember-me').checked;
    
    // Basic validation
    if (!username || !password) {
        showMessage("Please fill in all fields", true);
        return;
    }
    
    // Remember user if checkbox is checked
    if (rememberMe) {
        localStorage.setItem('rememberedUser', JSON.stringify({
            username: username,
            password: password
        }));
    } else {
        localStorage.removeItem('rememberedUser');
    }
    
    // Simulate API call delay
    setTimeout(() => {
        // Admin login check
        if (username === "admin" && password === "admin123") {
            showMessage("Admin login successful!", false);
            localStorage.setItem('isLoggedIn', 'true');
            localStorage.setItem('userData', JSON.stringify({
                username: username,
                isAdmin: true
            }));
            setTimeout(() => {
                window.location.href = "AdminDashboard.html";
            }, 500);
            return;
        }

        // Allow any username and password for user login
        if (username !== "admin") {
            showMessage("Login successful!", false);
            localStorage.setItem('isLoggedIn', 'true');
            localStorage.setItem('userData', JSON.stringify({
                username: username,
                isAdmin: false
            }));
            setTimeout(() => {
                window.location.href = "profile.html";
            }, 500);
        } else {
            showMessage("Invalid username or password", true);
        }
    }, 1000);
});

function showMessage(text, isError = true) {
    const messageLabel = document.querySelector('.message');
    if (messageLabel) {
        messageLabel.style.display = "block";
        messageLabel.textContent = text;
        messageLabel.style.color = isError ? "red" : "green";
    }
}

// Password requirements checking (unchanged)
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
        const li = document.getElementById(req);
        if (li) {
            if (requirements[req]) {
                li.classList.add('valid');
            } else {
                li.classList.remove('valid');
            }
        }
    });

    // Return true if all requirements are met
    return Object.values(requirements).every(req => req);
}

// Sign up form handling (unchanged)
document.addEventListener('DOMContentLoaded', function() {
    const form = document.getElementById('Form');
    const pass0 = document.getElementById('pass0');
    const pass1 = document.getElementById('pass1');
    const phoneInput = document.getElementById('phone');
    const messageElement = document.getElementById('message');
    let passwordValidation;

    if (pass0 && pass1) {
        // Initialize password validation
        passwordValidation = initPasswordValidation(pass0, pass1, messageElement);
    }

    function showMessage(text, isError = true) {
        if (!messageElement) return;
        
        messageElement.textContent = text;
        messageElement.style.display = 'block';
        messageElement.classList.toggle('success', !isError);
        if (!isError) {
            messageElement.style.color = '#24a17b';
            messageElement.style.background = 'rgba(36, 161, 123, 0.1)';
        } else {
            messageElement.style.color = '#ff3e3e';
            messageElement.style.background = 'rgba(255, 62, 62, 0.1)';
        }
    }

    function hideMessage() {
        if (messageElement) {
            messageElement.style.display = 'none';
        }
    }

    function isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }

    function isValidPhone(phone) {
        // Must start with 0 and have 9 more digits
        const phoneRegex = /^0[0-9]{9}$/;
        return phoneRegex.test(phone);
    }

    // Phone number input validation
    if (phoneInput) {
        phoneInput.addEventListener('input', function(e) {
            // Remove any non-digit characters
            this.value = this.value.replace(/\D/g, '');
            
            // Ensure it starts with 0
            if (this.value.length > 0 && this.value[0] !== '0') {
                this.value = '0' + this.value;
            }
            
            // Limit to 10 digits
            if (this.value.length > 10) {
                this.value = this.value.slice(0, 10);
            }

            // Show validation message
            if (this.value.length > 0) {
                if (this.value.length < 10) {
                    showMessage('Phone number must be 10 digits starting with 0');
                } else if (!isValidPhone(this.value)) {
                    showMessage('Invalid phone number format (must start with 0)');
                } else {
                    hideMessage();
                }
            } else {
                hideMessage();
            }
        });
    }

    // Confirm password validation
    if (pass1) {
        pass1.addEventListener('input', function() {
            if (pass0.value && this.value) {
                if (pass0.value !== this.value) {
                    showMessage('Passwords do not match');
                } else {
                    hideMessage();
                }
            }
        });
    }

    // Form submission
    if (form) {
        form.addEventListener('submit', function(event) {
            event.preventDefault();
            
            const username = document.getElementById('username')?.value;
            const email = document.getElementById('email')?.value;
            const phone = document.getElementById('phone')?.value;
            const studentId = document.getElementById('student-id')?.value;
            const password = pass0?.value;
            const confirmPass = pass1?.value;
            
            if (!username || !email || !phone || !studentId || !password || !confirmPass) {
                showMessage('Please fill in all fields');
                return;
            }

            if (!isValidEmail(email)) {
                showMessage('Please enter a valid email address');
                return;
            }

            if (!isValidPhone(phone)) {
                showMessage('Please enter a valid 10-digit phone number starting with 0');
                return;
            }

            if (passwordValidation && !passwordValidation.checkPasswordRequirements(password)) {
                showMessage('Please meet all password requirements');
                return;
            }
            
            if (password !== confirmPass) {
                showMessage('Passwords do not match');
                return;
            }

            // If validation passes, show success and redirect
            showMessage('Account created successfully!', false);
            
            // Disable the submit button and add loading state
            const submitButton = form.querySelector('input[type="submit"]');
            if (submitButton) {
                submitButton.disabled = true;
                submitButton.classList.add('loading');
            }
            
            // Simulate form submission delay
            setTimeout(() => {
                window.location.href = 'login.html';
            }, 1500);
        });
    }
});
