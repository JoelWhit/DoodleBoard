
(function () {
    communication.prototype.init = function (overlay, underlay) {

        var whiteboardId = document.getElementById('WhiteboardId').value;
        var modal = document.getElementById('myModal');
        var statusText = document.getElementById('connectionStatus');

        var buffer = [];

        $.connection.hub.starting(function () {
            console.log('connecting');
            modal.style.display = "block";
            statusText.innerText = 'connecting';
        });

        $.connection.hub.received(function () {
            console.log('received');
            modal.style.display = "none";
        });

        $.connection.hub.error(function (e) {
            console.log('error');
            console.log(e);
        });

        $.connection.hub.connectionSlow(function () {
            console.log('connection slow');
            modal.style.display = "none";
            statusText.innerText = 'connection slow';
        });

        $.connection.hub.reconnecting(function () {
            console.log('reconnecting');
            modal.style.display = "block";
            statusText.innerText = 'reconnecting';
        });

        $.connection.hub.reconnected(function () {
            console.log('reconnected');
            modal.style.display = "none";
            statusText.innerText = 'reconnected';
        });

        $.connection.hub.disconnected(function () {
            modal.style.display = "block";
            statusText.innerText = 'disconnected';
            console.log('disconnected');

            setTimeout(function () {

                console.log('attempting to connect');
                statusText.innerText = 'attempting to connect';
                $.connection.hub.start()

                    .done(function () {
                        console.log('connected');
                        statusText.innerText = 'connected';
                        modal.style.display = "none";
                    })
            }, 5000); // Restart connection after 5 seconds.
        });


        var connection = $.connection.doodleBoardHub;

        //regiser handlers before starting connection
        connection.client.recieveCoordinates = underlay.animator.recieveCoordinates;

        canvasOverlay.prototype.onDrawingFinish = function (drawing) {

            //add to buffer and remove when sent
            buffer.push(drawing);
            connection.server.send(whiteboardId, JSON.stringify(drawing))
                .done(function () {
                    buffer.splice(buffer.indexOf(drawing), 1);
                })
            return 
        }

        // $.connection.hub.logging = true;
        // verbose
        //$.connection.hub.stateChanged(connectionStateChanged);
        $.connection.hub.start()
            .done(function () {
                connection.server.join(whiteboardId)
                console.log('started connected');
            })
            .fail(function () {
                console.log('failed to start');
                statusText.innerText = 'failed to start';
            });

        window.onunload = function () { connection.server.leave(whiteboardId); }
    }
}());

function connectionStateChanged(state) {
    var stateConversion = { 0: 'connecting', 1: 'connected', 2: 'reconnecting', 4: 'disconnected' };
    console.log('SignalR state changed from: ' + stateConversion[state.oldState]
        + ' to: ' + stateConversion[state.newState]);
}