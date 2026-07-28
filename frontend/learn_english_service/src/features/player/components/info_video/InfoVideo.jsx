import LexicalComplexity from '../../../../ui/lexical_complexity/LexicalComplexity';
import styles from '../info_video/info_video.module.css';

function InfoVideo({title}){
    return(
        <div className={styles.info_video}>
            <h3>{title}</h3>
            <div style={{display:'flex', justifyContent:'center', alignContent:'center', width:'50px'}}>
                <LexicalComplexity/>
            </div>
        </div>
    )
}
export default InfoVideo;