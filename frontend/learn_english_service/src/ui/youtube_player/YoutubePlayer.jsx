import YouTube from 'react-youtube';
import styles from '../youtube_player/youtube_player.module.css';
import { useEffect, useRef } from 'react';

function YoutubePlayer({youtubeId, onTimeChange, onPlayerReady, startTime}){

    const playerRef = useRef(null);

    const onReady = (event) => {
        playerRef.current = event.target;
        onPlayerReady(event.target);

        if(startTime !== null){
            event.target.seekTo(startTime, true);
        }
    };

    useEffect(() => {
        if (!playerRef.current || startTime == null) return;

        playerRef.current.seekTo(startTime, true);
    }, [startTime]);

    useEffect(() => {
        const interval = setInterval(() => {
            if(!playerRef.current?.getCurrentTime) return; 
            
            if(playerRef.current){
                const time = playerRef.current.getCurrentTime();
                onTimeChange(time);
            }
        }, 400)

        return () => clearInterval(interval);
    }, []);


    return(
        <div className={styles.youtube_player}>
            <YouTube
                videoId={youtubeId}
                onReady={onReady}
                opts={{
                    width: "100%",
                    height: "100%",
                    playerVars:{
                        ...(startTime !== null ? {start: startTime} : {})
                    }
                }}
                style={{width: '100%', height: '100%'}}
            />
        </div>
    )
}

export default YoutubePlayer;