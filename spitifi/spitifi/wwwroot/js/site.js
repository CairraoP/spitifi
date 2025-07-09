// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener('DOMContentLoaded', function() {
    const rows = document.querySelectorAll('tbody tr');

    const albumRows = document.querySelectorAll('dl dd');
    
    rows.forEach(row => {
        row.addEventListener('click', function(event) {
            // ignora clicks em butões dentro da musica
            // exemplo: botao do like esta dentro do espaço do event listener par alterar musica
            // chutar like na musica não deve alterar para a musica
            if (event.target.closest('img[id^="heart-"]')) return;
            if (event.target.closest('audio')) return;

            const playlist = buildPlaylist();
            const songId = row.dataset.songId;

            // encontra indice da musica no array de musicas
            const songIndex = playlist.findIndex(song => song.id === songId);
            if (songIndex === -1) return;

            // inicializa musicaPlayer. Feito somente 1 vez
            if (!window.musicPlayer) {
                initPlayer(playlist);
            }
            window.musicPlayer.playTrack(songIndex);
        });
    });
});

// funcao para inicializar o musicPlayer. Corre quando a pagina é carregada
function initPlayer(playlist) {
    window.musicPlayer = {
        playlist: playlist,
        currentIndex: 0,
        audioPlayer: document.getElementById('footerAudioPlayer'),
        playMode: 'once',

        playTrack: function(index) {
            // validação inicial do indice
            if (index < 0 || index >= this.playlist.length) return;

            this.currentIndex = index;
            const track = this.playlist[index];

            // troca informações do footer de acordo com a nova musica
            document.getElementById('footerSongName').textContent =
                track.name.substring(0, track.name.lastIndexOf('.'));
            document.getElementById('footerArtist').textContent = track.artist;
            document.getElementById('footerAlbumArt').src = track.albumArt;

            // atualiza path da musica. Cada indice contem o seu proprio path para a musica (no wwwroot)
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

            // mostra footer. Escondido, por padrão
            const container = document.getElementById('audioContainer');
            container.classList.add('show');

        },
        
        // passa para a musica seguinte
        nextTrack: function() {
            let nextIndex = this.currentIndex + 1;

            if (nextIndex >= this.playlist.length) {
                if (this.playMode === 'loopPlaylist') nextIndex = 0;
                else return; // Stop at end
            }
            this.playTrack(nextIndex);
        },

        // passa para a musica anterior
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

    // modo padrão
    window.musicPlayer.setPlayMode('once');

    // adiciona eventListeners nos botões para saltar para musica anterior ou seguinte
    document.getElementById('musicaAnteriorBt').addEventListener('click', () => {
        window.musicPlayer.prevTrack();
    });

    document.getElementById('musicaSeguinteBt').addEventListener('click', () => {
        window.musicPlayer.nextTrack();
    });

    // escuta por mudanças na escolha do modo de playback (once, repeat, loop)
    document.querySelectorAll('input[name="playMode"]').forEach(radio => {
        radio.addEventListener('change', (e) => {
            window.musicPlayer.setPlayMode(e.target.value);
        });
    });

    // trata quando musica chega ao fim
    window.musicPlayer.audioPlayer.addEventListener('ended', () => {
        if (window.musicPlayer.playMode === 'loopPlaylist' || window.musicPlayer.playMode === 'once') {
            window.musicPlayer.nextTrack();
        }
    });
}