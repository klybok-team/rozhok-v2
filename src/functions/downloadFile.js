const fs = require('fs');

const axios = require('axios');

module.exports = async(url, path) => {
    const res = await axios({
        method: 'get',
        url: url,
        responseType: 'stream'
    });
    
    return await res.data.pipe(fs.createWriteStream(path));
};
