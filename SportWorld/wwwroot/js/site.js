// Facebook login
(function(d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = 'https://connect.facebook.net/en_US/sdk.js#xfbml=1&version=v3.1&appId=2036673273050380&autoLogAppEvents=1';
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

function checkLoginState() {
    FB.getLoginStatus((response) => statusChangeCallback(response))
}

function statusChangeCallback(response) {
    if (response.status === 'connected') {
        login();
    } else {
        $.ajax({ 
            url: `/User/Logout`, 
            type: 'GET' });
    }
}

function login() {
    FB.api('/me', (response) => {
        $.ajax({ 
            url: `/User/Login?id=${response.id}&name=${response.name}`, 
            type: 'GET' });
    });
}

window.fbAsyncInit = () => checkLoginState();