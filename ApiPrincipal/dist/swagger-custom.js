function createBtn() {
    if (document.getElementsByClassName('auth-wrapper')[0]) {
        const newButton = document.createElement('a');
        newButton.textContent = 'Manual de Usuario';
        newButton.href = 'https://bogota.gov.co/';
        newButton.target = '_blank';
        newButton.setAttribute('style', 'text-decoration: none;padding-top: 9px;');
        newButton.className = 'btn authorize';
        document.getElementsByClassName('auth-wrapper')[0].prepend(newButton);
    } else {
        setTimeout(createBtn, 100);
    }
}
//createBtn();