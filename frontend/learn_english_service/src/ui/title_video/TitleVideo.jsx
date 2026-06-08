import styles from '../title_video/title_video.module.css';

function TitleVideo({title}){
    return(
        <div className={styles.title}>
            {title}
        </div>
    )
}

export default TitleVideo;