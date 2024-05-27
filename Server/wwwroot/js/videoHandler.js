document.addEventListener('DOMContentLoaded', (event) => {
    const videoElement = document.querySelector('video.video-container');
    if (videoElement) {
        if (this.requestFullscreen) {
            this.requestFullscreen();
        } else if (this.mozRequestFullScreen) { // Firefox
            this.mozRequestFullScreen();
        } else if (this.webkitRequestFullscreen) { // Chrome, Safari and Opera
            this.webkitRequestFullscreen();
        } else if (this.msRequestFullscreen) { // IE/Edge
            this.msRequestFullscreen();
        }
    }
});
