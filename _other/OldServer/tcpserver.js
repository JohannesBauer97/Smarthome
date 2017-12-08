net = require('net');
var clients = [];


net.createServer(function (socket) {
  socket.name = socket.remoteAddress + ":" + socket.remotePort 
  clients.push(socket);

  socket.write(cmd,"ascii");

  socket.on('data', function (data) {
    console.log(data);
  });

  socket.on('end', function () {
    clients.splice(clients.indexOf(socket), 1);
  });

}).listen(5555);

console.log("Server running in port 5555");