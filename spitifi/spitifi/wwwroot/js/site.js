// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener('DOMContentLoaded', function() {
    const rows = document.querySelectorAll('tbody tr');

    const albumRows = document.querySelectorAll('dl dd');
    
    rows.forEach(row => {
        row.addEventListener('click', function(event) {
            // ignore clicks on like button and audio elements
            if (event.target.closest('img[id^="heart-"]')) return;
            if (event.target.closest('audio')) return;

            const playlist = buildPlaylist();
            const songId = row.dataset.songId;

            // find index of clicked song
            const songIndex = playlist.findIndex(song => song.id === songId);
            if (songIndex === -1) return;

            // initialize or update player
            if (!window.musicPlayer) {
                initPlayer(playlist);
            }
            window.musicPlayer.playTrack(songIndex);
        });
    });
});

// initialize the music player
function initPlayer(playlist) {
    window.musicPlayer = {
        playlist: playlist,
        currentIndex: 0,
        audioPlayer: document.getElementById('footerAudioPlayer'),
        playMode: 'once',

        playTrack: function(index) {
            if (index < 0 || index >= this.playlist.length) return;

            this.currentIndex = index;
            const track = this.playlist[index];

            // update display
            document.getElementById('footerSongName').textContent =
                track.name.substring(0, track.name.lastIndexOf('.'));
            document.getElementById('footerArtist').textContent = track.artist;
            document.getElementById('footerAlbumArt').src = track.albumArt;

            // set fallback for album art
            document.getElementById('footerAlbumArt').onerror = function() {
                this.src = '/assets/icons/musicPlaceholder.png';
            };

            // update audio source
            this.audioPlayer.innerHTML = '';
            const source = document.createElement('source');
            source.src = track.filePath;

            if (track.filePath.endsWith('.wav')) {
                source.type = 'audio/wav';
            } else if (track.filePath.endsWith('.mp3')) {
                source.type = 'audio/mp3';
            }

            this.audioPlayer.appendChild(source);
            this.audioPlayer.load();
            this.audioPlayer.play();

            // show footer music player
            const container = document.getElementById('audioContainer');
            container.classList.add('show');

        },


        nextTrack: function() {
            let nextIndex = this.currentIndex + 1;

            if (nextIndex >= this.playlist.length) {
                if (this.playMode === 'loopPlaylist') nextIndex = 0;
                else return; // Stop at end
            }
            this.playTrack(nextIndex);
        },

        prevTrack: function() {
            let prevIndex = this.currentIndex - 1;

            if (prevIndex < 0) {
                if (this.playMode === 'loopPlaylist') prevIndex = this.playlist.length - 1;
                else return; // Stop at start
            }
            this.playTrack(prevIndex);
        },

        setPlayMode: function(mode) {
            this.playMode = mode;
            this.audioPlayer.loop = (mode === 'loopTrack');
        }
    };

    // default play mode
    window.musicPlayer.setPlayMode('once');

    document.getElementById('prevBtn').addEventListener('click', () => {
        window.musicPlayer.prevTrack();
    });

    document.getElementById('nextBtn').addEventListener('click', () => {
        window.musicPlayer.nextTrack();
    });

    document.getElementById('playPauseBtn').addEventListener('click', () => {
        const icon = document.querySelector('#playPauseBtn i');
        if (window.musicPlayer.audioPlayer.paused) {
            window.musicPlayer.audioPlayer.play();
            icon.className = 'fas fa-pause';
        } else {
            window.musicPlayer.audioPlayer.pause();
            icon.className = 'fas fa-play';
        }
    });

    // play mode selection
    document.querySelectorAll('input[name="playMode"]').forEach(radio => {
        radio.addEventListener('change', (e) => {
            window.musicPlayer.setPlayMode(e.target.value);
        });
    });

    // handle track ending
    window.musicPlayer.audioPlayer.addEventListener('ended', () => {
        if (window.musicPlayer.playMode === 'loopPlaylist') {
            window.musicPlayer.nextTrack();
        } else if (window.musicPlayer.playMode === 'once') {
            window.musicPlayer.nextTrack();
        }
    });

    // update play/pause button state
    window.musicPlayer.audioPlayer.addEventListener('play', () => {
        document.querySelector('#playPauseBtn i').className = 'fas fa-pause';
    });

    window.musicPlayer.audioPlayer.addEventListener('pause', () => {
        document.querySelector('#playPauseBtn i').className = 'fas fa-play';
    });
}