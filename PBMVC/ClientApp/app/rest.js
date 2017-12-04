
export default {
    createAlbum: function (ok) {
        var data = {
            method: 'POST',
            url: '/api/album',
            ok: ok
        };
        this.json(data);
    },
    getImagesByAlbumId: function (id, ok) {
        var data = {
            method: 'GET',
            url: '/api/album/images?id=' + id,
            ok: ok
        };
        this.json(data);
    },
    getAlbum: function (albumId, ok) {
        var data = {
            method: 'GET',
            url: '/api/album/album?albumId=' + albumId,
            ok: ok
        };
        this.json(data);
    },
    listAlbumsByUserId: function (userId, ok) {
        var data = {
            method: 'GET',
            url: '/api/album/albums?userId=' + userId,
            ok: ok
        };
        this.json(data);
    },
    deleteAlbum: function (albumId, ok) {
        var data = {
            method: 'DELETE',
            url: '/api/album/albums?albumId=' + albumId,
            ok: ok
        };
        this.json(data);
    },
    shareAlbumWith: function (albumId, userId, ok) {
        var data = {
            method: 'POST',
            url: '/api/album/share-with?userId=' + userid + '&albumId=' + albumId,
            ok: ok
        };
        this.json(data);
    },
    revokeAlbum: function (albumId, userId, ok) {
        var data = {
            method: 'POST',
            url: '/api/album/revoke-share?userId=' + userId + '&albumId=' + albumId,
            ok: ok
        };
        this.json(data);
    },
    deletePicture: function (imageId, ok) {
        var data = {
            method: 'DELETE',
            url: '/api/picture?imageId=' + imageId,
            ok: ok
        };
        this.json(data);
    },
    editPicture: function (image, ok) {
        var data = {
            body: image,
            method: 'PUT',
            url: '/api/picture',
            ok: ok
        };
        this.json(data);
    },
    getUserInfo: function (userId, ok) {
        var data = {
            method: 'GET',
            url: '/api/user?userId=' + userId,
            ok: ok
        };
        this.json(data);
    },
    editSelf: function (user, ok) {
        var data = {
            body: user,
            method: 'POST',
            url: '/api/user',
            ok: ok
        };
        this.json(data);
    },

    json: function (data) {
        var xhr = new XMLHttpRequest();
        xhr.open(data.method, encodeURI(data.url));
        xhr.setRequestHeader('Content-Type', 'application/json');

        xhr.onreadystatechange = function () {
            var DONE = 4, OK = 200;
            if (xhr.readyState === DONE) {
                if (xhr.status === OK && data.ok !== undefined) {
                    if (xhr.responseText !== undefined && xhr.responseText !== "") {
                        data.ok(JSON.parse(xhr.responseText));
                    } else {
                        data.ok();
                    }
                }
            }
        };
        var body = undefined;
        if (data.body !== undefined) {
            body = JSON.stringify(data.body);
        }
        xhr.send(body);
    }
};