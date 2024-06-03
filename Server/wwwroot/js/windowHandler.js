window.getWindowDimensions = function () {
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
};

window.getCurrentBreakpoint = function () {
    var width = window.innerWidth;
    if (width >= 2560) {
        return 'xx';
    } else if (width >= 1920) {
        return 'xl';
    } else if (width >= 1280) {
        return 'lg';
    } else if (width >= 1024) {
        return 'md';
    } else if (width >= 768) {
        return 'sm';
    } else if (width >= 576) {
        return 'xs';
    } else {
        return null;
    }
};

window.notifyViewportChanged = function (dotNetHelper, dimension) {
    dotNetHelper.invokeMethodAsync('ViewportChanged', dimension)
        .then(data => console.log("ViewportChanged event sent", data))
        .catch(err => console.error("Error sending ViewportChanged event", err));
};

window.registerResizeEvent = function (dotNetHelper) {
    window.addEventListener('resize', function () {
        var dimensions = window.getWindowDimensions();
        window.notifyViewportChanged(dotNetHelper, dimensions);
    });

    // Initial Viewport Change Event
    var dimensions = window.getWindowDimensions();
    window.notifyViewportChanged(dotNetHelper, dimensions);
};
