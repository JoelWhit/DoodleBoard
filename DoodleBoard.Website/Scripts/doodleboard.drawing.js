
function createCanvasOverlay(parentCanvas, overlayId, zindex) {

    var shadowCanvas = document.createElement('canvas');
    shadowCanvas.id = overlayId;
    shadowCanvas.style.position = 'absolute';
    shadowCanvas.style.backgroundColor = 'white';
    shadowCanvas.width = parentCanvas.width;
    shadowCanvas.height = parentCanvas.height;
    shadowCanvas.style.top = parentCanvas.offsetTop + Number.parseInt(window.getComputedStyle(parentCanvas).marginTop.slice(0, -2)) + 'px';
    shadowCanvas.style.left = parentCanvas.offsetLeft + Number.parseInt(window.getComputedStyle(parentCanvas).marginLeft.slice(0, -2)) + 'px';
    shadowCanvas.style.zIndex = zindex;
    shadowCanvas.style.pointerEvents = 'none';

    parentCanvas.insertAdjacentElement('afterend', shadowCanvas);

    return shadowCanvas;
}


// communication implementation
function communication() {
}
communication.prototype.init = function (overlay, underlay) {
};

function doodleboard() {
    var self = this;
    var canvas = document.getElementById('doodleboard');
    var shadowCanvas = createCanvasOverlay(canvas, 'shadowCanvas', 1)

    canvas.style.zIndex = 1000;


    $(window).resize(function () {
        shadowCanvas.style.top = canvas.offsetTop + Number.parseInt(window.getComputedStyle(canvas).marginTop.slice(0, -2)) + 'px';
        shadowCanvas.style.left = canvas.offsetLeft + Number.parseInt(window.getComputedStyle(canvas).marginLeft.slice(0, -2)) + 'px';
    });


    this.overlay = new canvasOverlay(canvas, shadowCanvas);
    this.underlay = new canvasUnderlay(shadowCanvas);

    var comms = new communication();
    comms.init(this.overlay, this.underlay);

    function changeShape(shape) {
        this.overlay.shape = shape;
    }
}


function canvasInteraction(canvas) {
    this.canvas = canvas;
    this.ctx = canvas.getContext("2d");
    this.ctx.lineJoin = "round";
    this.ctx.lneCap = "round";

    this.buffer = [];

    this.clear = function () {
        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
    }
}


function canvasDrawer(canvas) {

    canvasInteraction.call(this, canvas);
    var self = this;

    this.draw = function (cords) {
        this.ctx.strokeStyle = cords.strokeStyle;;
        this.ctx.lineWidth = cords.lineWidth;
        this.ctx.fillStyle = cords.fillStyle;

        switch (cords.shape) {
            case 'path':
                drawPath(cords)
                break;
            case 'line':
                drawLine(cords)
                break;
            case 'circle':
                drawCircle(cords)
                break;
            case 'oval':
                drawOval(cords)
                break;
            case 'rectangle':
                drawRectangle(cords)
                break;
            default:
                console.log("unknown shape: " + cords.shape)
        }

    }

    function drawPath(cords) {

        for (var i = 1; i < cords.coordinates.length; i++) {
            self.ctx.beginPath();
            self.ctx.moveTo(cords.coordinates[i - 1].X, cords.coordinates[i - 1].Y);
            self.ctx.lineTo(cords.coordinates[i].X, cords.coordinates[i].Y);
            self.ctx.closePath();
        }

        self.ctx.stroke();
    }
    function drawLine(cords) {

        self.ctx.beginPath();
        self.ctx.moveTo(cords.coordinates[0].X, cords.coordinates[0].Y);
        self.ctx.lineTo(cords.coordinates[1].X, cords.coordinates[1].Y);
        self.ctx.closePath();
        self.ctx.stroke();
    }
    function drawCircle(cords) {
        //mid points
        var height = (cords.coordinates[1].Y - cords.coordinates[0].Y) / 2;
        var width = (cords.coordinates[1].X - cords.coordinates[0].X) / 2;


        var adj = height + (height / 2);
        var opp = width + (width / 2);

        var hyp = adj * adj + opp * opp;
        var radius = Math.sqrt(hyp) / 2;

        self.ctx.beginPath();
        self.ctx.arc(cords.coordinates[0].X + width, cords.coordinates[0].Y + height, radius, 0, Math.PI * 2);
        if (cords.fill) { self.ctx.fill(); }
        self.ctx.stroke();
        self.ctx.closePath();
    }
    function drawOval(cords) {
        /*
        C4------A1-----C1
        |               |
        |               |
        |               |
        |               |
        |               |
        C3-----A2------C2    
        */
        var height = Math.abs(cords.coordinates[0].Y - cords.coordinates[1].Y);
        var width = Math.abs(cords.coordinates[0].X - cords.coordinates[1].X);

        var centerX = ((cords.coordinates[1].X - cords.coordinates[0].X) / 2) + cords.coordinates[0].X;
        var centerY = ((cords.coordinates[1].Y - cords.coordinates[0].Y) / 2) + cords.coordinates[0].Y;

        self.ctx.beginPath();
        self.ctx.moveTo(centerX, centerY - height / 2); // A1

        self.ctx.bezierCurveTo(
            centerX + width / 2, centerY - height / 2, // C1
            centerX + width / 2, centerY + height / 2, // C2
            centerX, centerY + height / 2); // A2

        self.ctx.bezierCurveTo(
            centerX - width / 2, centerY + height / 2, // C3
            centerX - width / 2, centerY - height / 2, // C4
            centerX, centerY - height / 2); // A1

        self.ctx.stroke();
        if (cords.fill) { self.ctx.fill(); }
        self.ctx.closePath();
    }
    function drawRectangle(cords) {
        var height = (cords.coordinates[0].Y - cords.coordinates[1].Y) * -1;
        var width = (cords.coordinates[0].X - cords.coordinates[1].X) * -1;
        //this.ctx.fillRect(this.cords[0].X, this.cords[0].Y, width, height);
        self.ctx.beginPath();
        self.ctx.rect(cords.coordinates[0].X, cords.coordinates[0].Y, width, height);
        self.ctx.stroke();
        if (cords.fill) { self.ctx.fill(); }
        self.ctx.closePath();
    }
}
canvasDrawer.prototype = Object.create(canvasInteraction.prototype)


function canvasAnimator(canvas) {
    canvasDrawer.call(this, canvas);

    var self = this;
    var renderframes = 2; // frames to render per cycle
    var interval = 50;

    
    animationLoop();

    this.recieveCoordinates = function (cords) {

        var coordinates = transformIntoAnimation(JSON.parse(cords));
        self.buffer.push(coordinates);
    }

    function transformIntoAnimation(cords) {
        cords.animationIndex = 0;
        return cords;
    }

    function animationLoop() {
        if (self.buffer.length == 0) {
            //avoid long running javascript warnings
            setTimeout(animationLoop, interval);
        } else {
            requestAnimFrame(animationLoop)
            animation: // render animation buffer
            for (var i = 0; i < self.buffer.length; i++) {
                var coords = self.buffer[i];
                frame: // render multiple coordinate frames per refresh cycle
                for (var frame = 0; frame < renderframes; frame++) {
                    if (coords.animationIndex + 1 > coords.coordinates.length - 1) {
                        //animation finished remove from buffer
                        self.buffer.splice(i, 1);
                        i--;
                        continue animation;
                    }

                    self.ctx.strokeStyle = coords.strokeStyle;
                    self.ctx.lineWidth = coords.lineWidth;
                    var idx = coords.animationIndex;

                    self.ctx.beginPath();
                    self.ctx.moveTo(coords.coordinates[idx].X, coords.coordinates[idx].Y);
                    self.ctx.lineTo(coords.coordinates[idx + 1].X, coords.coordinates[idx + 1].Y);
                    self.ctx.closePath();
                    self.ctx.stroke();

                    coords.animationIndex++;
                }
            }
        }
    }
}
canvasAnimator.prototype = Object.create(canvasDrawer.prototype)
canvasAnimator.prototype.draw = function () {
    for (var i = 0; i < this.buffer.length; i++) {
        switch (this.buffer[i].shape) {
            case 'path':
                self.drawPath(this.buffer[i])
            case 'line':
                self.drawLine(this.buffer[i])
            case 'circle':
                self.drawCircle(this.buffer[i])
            case 'oval':
                self.drawOval(this.buffer[i])
            case 'rectangle':
                self.drawRectangle(this.buffer[i])
            default:
                console.log("unknown shape: " + this.buffer[i].shape)
        }
        // animation index check
    }
}


function canvasOverlay(canvas, underlayCanvas) {

    var self = this;

    var overlay = new canvasDrawer(canvas);
    var underlay = new canvasDrawer(underlayCanvas);

    var isDrawing = false;
    var bufferLimit = 50;
    var buffer = [];

    this.lineWidth = overlay.ctx.lineWidth;
    this.strokeStyle = overlay.ctx.strokeStyle;
    this.fillStyle = overlay.ctx.fillStyle;
    this.useFill = false;

    this.shape = "path";

    $(canvas).on('mousedown mouseup mousemove mouseleave mouseout touchstart touchmove touchend touchcancel', onEvent);

    $("body").keypress(function () {
        cancelDrawing();
    });

    function cancelDrawing() {
        isDrawing = false;
        buffer = [];
    }

    function calculateCoordinates(e) {
        var borderX = window.getComputedStyle(canvas).getPropertyValue('border-left-width').slice(0, -2);
        var borderY = window.getComputedStyle(canvas).getPropertyValue('border-top-width').slice(0, -2);

        var scrollX = document.body.scrollLeft;
        var scrollY = document.body.scrollTop;

        var canvasOffset = $(canvas).offset();

        var X = e.clientX - parseInt(canvasOffset.left) - borderX + parseInt(scrollX);
        var Y = e.clientY - parseInt(canvasOffset.top) - borderY + parseInt(scrollY);

        var box = canvas.getBoundingClientRect();

        return { X: X, Y: Y };
    }

    function onEvent(e) {
        var cords = calculateCoordinates(e);
        switch (e.type) {
            case 'mousemove':
            case 'touchmove':
                //fires when stationary apparently
                if (isDrawing && buffer[0].X != cords.X && buffer[0].Y != cords.Y) {
                    if (self.shape != 'path') {
                        buffer[1] = cords;
                    }
                    else {
                        buffer.push(cords);
                    }
                    bufferLimitCheck()
                    redraw();
                }
                break;
            case 'mousedown':
            case 'touchstart':
                switch (e.which) {
                    case 1:
                        isDrawing = true;
                        buffer[0] = cords;
                }
                break;
            case 'mouseup':
            case 'mouseout':
            case 'mouseleave':
            case 'touchend':
            case 'touchcancel':
                if (buffer.length > 1) {
                    if (self.shape != 'path') {
                        buffer[1] = cords;
                    }
                    else {
                        buffer.push(cords);
                    }
                    sendCoordinates();
                    isDrawing = false;
                }
                break;
        }
    }


    function redraw() {
        if (self.shape != 'path') {
            overlay.clear();
        }
        overlay.draw(createCoordinates())
    }

    // will preemptively send when buffer gets too large
    function bufferLimitCheck() {
        if (buffer.length > bufferLimit) {
            var lastCord = buffer.slice(-1)[0];
            sendCoordinates();
            buffer[0] = lastCord;
        }
    }

    function purge() {
        buffer = [];
        overlay.clear();
    }

    function sendCoordinates() {
        var cords = createCoordinates();
        self.onDrawingFinish(cords);
        underlay.draw(cords)
        purge()
    }

    function createCoordinates() {
        return new drawingCoordinates(self.strokeStyle, self.fillStyle, self.lineWidth, buffer, self.shape);
    }
}
canvasOverlay.prototype.onDrawingFinish = function (cords) {
};


function canvasUnderlay(canvas) {
    this.canvas = canvas;
    this.ctx = canvas.getContext("2d");
    this.drawer = new canvasDrawer(canvas);
    this.animator = new canvasAnimator(canvas);




}


function drawingCoordinates(strokeStyle, fillStyle, lineWidth, coordinates, shape) {
    this.strokeStyle = strokeStyle;
    this.fillStyle = fillStyle
    this.lineWidth = lineWidth;
    this.coordinates = coordinates;
    //this.animationIndex = 0;
    this.shape = shape
    this.fill = (fillStyle != 'none');
}

//https://www.paulirish.com/2011/requestanimationframe-for-smart-animating/
// shim layer with setTimeout fallback
window.requestAnimFrame = (function () {
    return window.requestAnimationFrame ||
        window.webkitRequestAnimationFrame ||
        window.mozRequestAnimationFrame ||
        function (callback) {
            window.setTimeout(callback, 1000 / 60);
        };
})();