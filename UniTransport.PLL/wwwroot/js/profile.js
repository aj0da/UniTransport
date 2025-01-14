//// Load user data when the page loads
//document.addEventListener('DOMContentLoaded', function() {
//    loadUserData();
//});

// Load user data from localStorage
function loadUserData() {
    const userData = JSON.parse(localStorage.getItem('userData')) || {};
    document.getElementById('profile-user-name').textContent = userData.name || 'Not set';
    document.getElementById('profile-user-email').textContent = userData.email || 'Not set';
    document.getElementById('profile-user-id').textContent = userData.id || 'Not set';
}

// Toggle edit mode
function toggleEditMode() {
    const userInfo = document.querySelector('.user-info');
    const isEditing = userInfo.classList.contains('editing');

    if (!isEditing) {
        // Switch to edit mode
        const userData = JSON.parse(localStorage.getItem('userData')) || {};
        
        // Replace spans with input fields
        document.getElementById('profile-user-name').innerHTML = `
            <input type="text" id="profile-edit-name" value="${userData.name || ''}" placeholder="Enter your name">
        `;
        document.getElementById('profile-user-email').innerHTML = `
            <input type="email" id="profile-edit-email" value="${userData.email || ''}" placeholder="Enter your email">
        `;
        document.getElementById('profile-user-id').innerHTML = `
            <input type="text" id="profile-edit-id" value="${userData.id || ''}" placeholder="Enter your ID">
        `;

        // Change button text
        document.querySelector('.update-button').textContent = 'Save Changes';
        userInfo.classList.add('editing');
    } else {
        // Save changes
        const newData = {
            name: document.getElementById('profile-edit-name').value,
            email: document.getElementById('profile-edit-email').value,
            id: document.getElementById('profile-edit-id').value
        };

        // Validate data
        if (!newData.name || !newData.email || !newData.id) {
            alert('Please fill in all fields');
            return;
        }

        if (!isValidEmail(newData.email)) {
            alert('Please enter a valid email address');
            return;
        }

        // Save to localStorage
        localStorage.setItem('userData', JSON.stringify(newData));

        // Switch back to display mode
        loadUserData();
        document.querySelector('.update-button').textContent = 'Update';
        userInfo.classList.remove('editing');

        // Show success message
        showMessage('Profile updated successfully!');
    }
}

// Validate email format
function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

// Show message function
function showMessage(message) {
    const messageDiv = document.createElement('div');
    messageDiv.className = 'message success';
    messageDiv.textContent = message;
    
    document.querySelector('.user-info').appendChild(messageDiv);
    
    // Remove message after 3 seconds
    setTimeout(() => {
        messageDiv.remove();
    }, 3000);
}

// Handle profile image upload
function loadImage(event) {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function(e) {
            document.getElementById('profile-image').src = e.target.result;
            // Save image to localStorage
            localStorage.setItem('profileImage', e.target.result);
        };
        reader.readAsDataURL(file);
    }
}
