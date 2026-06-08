import styles from '../videos_skeleton/videos_skeleton.module.css';

function VideosSkeleton(){
    return(
        <>
            {Array.from({length: 5}).map((_, index) => (
                <div 
                    key={index}
                    className={styles.video_skeleton}
                ></div>
            ))}
        </>
    )
}

export default VideosSkeleton;