import styles from '../preview_video/preview_video.module.css';

function PreviewVideo({imgUrl}){
    return(
        <img className={styles.img} src={imgUrl} alt='...'/>
    )
}

export default PreviewVideo;