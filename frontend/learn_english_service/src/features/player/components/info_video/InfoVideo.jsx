import LexicalComplexity from '../../../../ui/lexical_complexity/LexicalComplexity';
import styles from '../info_video/info_video.module.css';

function InfoVideo({video}){
    return(
        <div className={styles.info_video}>
            <h3>{video.titleVideo}</h3>
            <div style={{display:'flex', justifyContent:'center', alignContent:'center', width:'50px'}}>
                <LexicalComplexity lexicalComplexity={video.lexicalComplexity}/>
            </div>
        </div>
    )
}
export default InfoVideo;