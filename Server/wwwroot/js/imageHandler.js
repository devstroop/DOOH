window.checkImage = async function (url) {
    try {
        if (!url) {
            return false;
        }
        if (url.startsWith('/')) {
            url = window.location.origin + url;
        }
        const response = await fetch(url, { method: 'HEAD' });
        return response.status === 200;
    } catch {
        return false;
    }
};