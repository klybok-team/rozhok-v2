const fs = require('fs');

module.exports = async(args, boolean, guildID) => {
    let path = `../data/data.txt`;
    if(boolean === false) {
        path = `../data/${guildID}_data.txt`
    }
    let buffer = new Buffer.from(`\n${args}`);
    return fs.open(path, 'a', function(err, fd) {
        if(err) {
            console.error(err)
        } else {
            fs.write(fd, buffer, 0, buffer.length, 
                    null, function(err,writtenbytes) {
                if(err) {
                    console.error(err);
                } else {
                    console.log(writtenbytes +
                        ' символов добавлено в файл');
                }
            })
        }
});
};