// Check login status when page loads
document.addEventListener('DOMContentLoaded', function() {
    // Don't show logout button on home page
    if (!window.location.href.includes('h.html')) {
        updateLoginStatus();
    }
});

function updateLoginStatus() {
    const isLoggedIn = localStorage.getItem('isLoggedIn') === 'true';
    const navElement = document.querySelector('nav');
    
    if (navElement) {
        const loginLink = navElement.querySelector('.login-link');
        if (isLoggedIn) {
            if (loginLink) {
                loginLink.textContent = 'Log out';
                loginLink.classList.remove('login-link');
                loginLink.classList.add('logout-link');
                loginLink.addEventListener('click', handleLogout);
                
                // If on login page and user is logged in, redirect to profile
                if (window.location.href.includes('login.html')) {
                    window.location.href = 'profile.html';
                }
            }
        } else {
            // If not logged in and not on login page, redirect to login
            if (!window.location.href.includes('login.html') && 
                !window.location.href.includes('h.html') && 
                !window.location.href.includes('sign.html')) {
                window.location.href = 'login.html';
            }
            
            if (loginLink) {
                loginLink.textContent = 'Log in';
                loginLink.classList.remove('logout-link');
                loginLink.classList.add('login-link');
                loginLink.href = 'login.html';
            }
        }
    }
}

function handleLogout(e) {
    e.preventDefault();
    localStorage.removeItem('isLoggedIn');
    localStorage.removeItem('userData');
    window.location.href = 'login.html';
}
