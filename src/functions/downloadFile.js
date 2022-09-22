const fs = require('fs');

const axios = require('axios');

module.exports = async(url, path) => {
    const res = await axios({
        method: 'get',
        url: url,
        responseType: 'stream'
    });
    
    if(res.data.headers['content-type'] === 'image/jpg') {
        res.data.headers['content-type'] = 'image/jpeg'
    };

    return await res.data.pipe(fs.createWriteStream(path));
};
